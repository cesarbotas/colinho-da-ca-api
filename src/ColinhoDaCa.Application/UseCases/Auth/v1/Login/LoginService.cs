using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.LoginHistorico.Entities;
using ColinhoDaCa.Domain.LoginHistorico.Repositories;
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
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(ILogger<LoginService> logger,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IPasswordService passwordService,
        IJwtService jwtService,
        ILoginHistoricoRepository loginHistoricoRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _loginHistoricoRepository = loginHistoricoRepository;
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

            var token = _jwtService.GenerateToken(usuarioResponse);

            // Gravar histórico de login
            var loginHistorico = new LoginHistoricoDb
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

            await _loginHistoricoRepository.AddAsync(loginHistorico);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse
            {
                Token = token,
                //Usuario = usuarioResponse
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Login");
            throw;
        }
    }
}
