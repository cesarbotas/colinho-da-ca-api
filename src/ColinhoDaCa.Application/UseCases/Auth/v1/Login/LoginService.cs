using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public class LoginService : ILoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public LoginService(ILogger<LoginService> logger,
        IUsuarioRepository usuarioRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(LoginCommand command)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(command.Email);

            if (usuario == null || !_passwordService.VerifyPassword(command.Senha, usuario.SenhaHash))
            {
                throw new Exception("Email ou senha inválidos");
            }

            var token = _jwtService.GenerateToken(usuario.Email, usuario.Id);

            return new LoginResponse
            {
                Token = token,
                Usuario = new UsuarioResponse
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Login");
            throw;
        }
    }
}
