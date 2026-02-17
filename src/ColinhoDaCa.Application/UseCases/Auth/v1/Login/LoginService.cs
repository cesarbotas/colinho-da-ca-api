using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.LoginHistoricos.Entities;
using ColinhoDaCa.Domain.LoginHistoricos.Repositories;
using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public class LoginService : ILoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly ILoginHistoricoRepository _loginHistoricoRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(ILogger<LoginService> logger,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IPasswordService passwordService,
        IJwtService jwtService,
        ILoginHistoricoRepository loginHistoricoRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _loginHistoricoRepository = loginHistoricoRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(LoginCommand command)
    {
        try
        {
            var cliente = await _clienteRepository.GetByEmailAsync(command.Email);

            if (cliente == null)
            {
                throw new ValidationException("Email ou senha inválidos");
            }

            var usuario = await _usuarioRepository.GetByClienteIdWithPerfisAsync(cliente.Id);

            if (usuario == null || !usuario.Ativo || !_passwordService.VerifyPassword(command.Senha, usuario.SenhaHash))
            {
                throw new ValidationException("Email ou senha inválidos");
            }

            var perfis = await _usuarioRepository.GetPerfisUsuarioAsync(usuario.Id);

            var usuarioResponse = new UsuarioResponse
            {
                Id = usuario.Id,
                ClienteId = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Celular = cliente.Celular,
                Cpf = cliente.Cpf,
                Perfis = perfis.Select(p => new PerfilResponse
                {
                    Id = p.Id,
                    Nome = p.Nome
                }).ToList()
            };

            var accessToken = _jwtService.GenerateAccessToken(usuarioResponse);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Revogar tokens anteriores do usuário
            await _refreshTokenRepository.RevokeAllUserTokensAsync(usuario.Id);

            // Criar novo refresh token
            var refreshTokenEntity = new RefreshToken
            {
                UsuarioId = usuario.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 dias
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.InsertAsync(refreshTokenEntity);

            // Gravar histórico de login
            var loginHistorico = new LoginHistorico
            {
                UsuarioId = usuario.Id,
                Email = command.Email,
                UserAgent = command.DeviceInfo?.UserAgent,
                Platform = command.DeviceInfo?.Platform,
                Language = command.DeviceInfo?.Language,
                ScreenResolution = command.DeviceInfo?.ScreenResolution,
                Timezone = command.DeviceInfo?.Timezone,
                ClientIP = command.DeviceInfo?.ClientIP,
                DataLogin = DateTime.UtcNow
            };

            await _loginHistoricoRepository.InsertAsync(loginHistorico);
            await _unitOfWork.CommitAsync();

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Login");
            throw;
        }
    }
}