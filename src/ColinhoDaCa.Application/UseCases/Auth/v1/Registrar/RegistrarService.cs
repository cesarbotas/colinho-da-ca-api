using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;

public class RegistrarService : IRegistrarService
{
    private readonly ILogger<RegistrarService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordService _passwordService;

    public RegistrarService(ILogger<RegistrarService> logger,
        IUnitOfWork unitOfWork,
        IUsuarioRepository usuarioRepository,
        IPasswordService passwordService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
    }

    public async Task Handle(RegistrarCommand command)
    {
        try
        {
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(command.Email);

            if (usuarioExistente != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var senhaHash = _passwordService.HashPassword(command.Senha);
            var usuario = UsuarioDb.Create(command.Nome, command.Email, senhaHash);

            await _usuarioRepository.InsertAsync(usuario);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Registro de Usuário");
            throw;
        }
    }
}
