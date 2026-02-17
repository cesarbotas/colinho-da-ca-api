using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.RefreshTokens;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ILogger<RefreshTokenService> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(ILogger<RefreshTokenService> logger,
        IRefreshTokenRepository refreshTokenRepository,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand command)
    {
        try
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken);
            
            if (refreshToken == null)
            {
                throw new ValidationException("Refresh token inválido");
            }

            var usuario = await _usuarioRepository.GetAsync(refreshToken.UsuarioId);
            if (usuario == null || !usuario.Ativo)
            {
                throw new ValidationException("Usuário inválido");
            }

            var cliente = await _clienteRepository.GetAsync(usuario.ClienteId);
            if (cliente == null)
            {
                throw new ValidationException("Cliente não encontrado");
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

            // Gerar novos tokens
            var newAccessToken = _jwtService.GenerateAccessToken(usuarioResponse);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Revogar token atual
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            // Criar novo refresh token
            var newRefreshTokenEntity = new RefreshToken
            {
                UsuarioId = usuario.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.InsertAsync(newRefreshTokenEntity);
            await _unitOfWork.CommitAsync();

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Refresh Token");
            throw;
        }
    }
}