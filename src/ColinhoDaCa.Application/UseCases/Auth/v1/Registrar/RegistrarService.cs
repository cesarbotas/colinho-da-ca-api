using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Validation;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;

public class RegistrarService : IRegistrarService
{
    private readonly ILogger<RegistrarService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPasswordService _passwordService;
    private readonly ICpfValidationService _cpfValidationService;

    public RegistrarService(ILogger<RegistrarService> logger,
        IUnitOfWork unitOfWork,
        IUsuarioRepository usuarioRepository,
        IClienteRepository clienteRepository,
        IPasswordService passwordService,
        ICpfValidationService cpfValidationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _usuarioRepository = usuarioRepository;
        _clienteRepository = clienteRepository;
        _passwordService = passwordService;
        _cpfValidationService = cpfValidationService;
    }

    public async Task Handle(RegistrarCommand command)
    {
        try
        {
            if (!_cpfValidationService.IsValid(command.Cpf))
            {
                throw new Exception("CPF inválido");
            }

            var clienteExistente = await _clienteRepository.GetByCpfAsync(command.Cpf);

            if (clienteExistente != null)
            {
                throw new Exception("CPF já cadastrado");
            }

            var cliente = ClienteDb.Create(command.Nome, command.Email, command.Celular, command.Cpf, "");
            await _clienteRepository.InsertAsync(cliente);
            await _unitOfWork.CommitAsync();

            var senhaHash = _passwordService.HashPassword(command.Senha);
            var usuario = UsuarioDb.Create(senhaHash, cliente.Id);
            await _usuarioRepository.InsertAsync(usuario);
            await _unitOfWork.CommitAsync();

            var perfilCliente = UsuarioPerfilDb.Create(usuario.Id, 2);
            usuario.AdicionarPerfil(perfilCliente);
            _usuarioRepository.Update(usuario);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema no Registro de Usuário");
            throw;
        }
    }
}
