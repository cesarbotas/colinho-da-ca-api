using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;

public class CadastrarClienteService : ICadastrarClienteService
{
    private readonly ILogger<CadastrarClienteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;

    public CadastrarClienteService(ILogger<CadastrarClienteService> logger,
        IUnitOfWork unitOfWork,
        IClienteRepository clienteRepository,
        IUsuarioRepository usuarioRepository,
        IPasswordService passwordService,
        IEmailService emailService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _clienteRepository = clienteRepository;
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
        _emailService = emailService;
    }

    public async Task Handle(CadastrarClienteCommand command)
    {
        try
        {
            var clienteExistenteEmail = await _clienteRepository.GetByEmailAsync(command.Email);
            if (clienteExistenteEmail != null)
            {
                throw new ValidationException("Email já cadastrado");
            }

            var clienteExistenteCpf = await _clienteRepository.GetByCpfAsync(command.Cpf);
            if (clienteExistenteCpf != null)
            {
                throw new ValidationException("CPF já cadastrado");
            }

            var cliente = Cliente.Create(command.Nome, command.Email, command.Celular, command.Cpf, command.Observacoes);

            await _clienteRepository.InsertAsync(cliente);
            await _unitOfWork.CommitAsync();

            // Generate default password
            var defaultPassword = GenerateDefaultPassword();
            var hashedPassword = _passwordService.HashPassword(defaultPassword);

            // Create user account
            var usuario = Usuario.Create(cliente.Id, hashedPassword);
            await _usuarioRepository.InsertAsync(usuario);
            await _unitOfWork.CommitAsync();

            // Send email with credentials
            await _emailService.EnviarEmailAsync(
                cliente.Email,
                "Conta criada - Colinho da Cá",
                $"Sua conta foi criada.\nEmail: {cliente.Email}\nSenha temporária: {defaultPassword}\n\nAltere sua senha no primeiro acesso.");

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Inclusão de Clientes");

            throw;
        }
    }

    private string GenerateDefaultPassword()
    {
        var random = new Random();
        return $"Temp{random.Next(1000, 9999)}!";
    }
}