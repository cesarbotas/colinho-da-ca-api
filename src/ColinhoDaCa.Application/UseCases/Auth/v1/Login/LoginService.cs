using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
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

    public LoginService(ILogger<LoginService> logger,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
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
