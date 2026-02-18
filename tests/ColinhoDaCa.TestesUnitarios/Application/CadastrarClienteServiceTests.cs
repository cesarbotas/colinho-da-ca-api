using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application;

public class CadastrarClienteServiceTests
{
    private readonly Mock<ILogger<CadastrarClienteService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly CadastrarClienteService _service;

    public CadastrarClienteServiceTests()
    {
        _loggerMock = new Mock<ILogger<CadastrarClienteService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _emailServiceMock = new Mock<IEmailService>();

        _service = new CadastrarClienteService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _clienteRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _passwordServiceMock.Object,
            _emailServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateClienteAndUsuario()
    {
        var command = new CadastrarClienteCommand
        {
            Nome = "Test Client",
            Email = "test@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Observacoes = "Test"
        };

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((Cliente?)null);
        _clienteRepositoryMock.Setup(x => x.GetByCpfAsync(command.Cpf))
            .ReturnsAsync((Cliente?)null);
        _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>()))
            .Returns("hashedPassword");

        await _service.Handle(command);

        _clienteRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Cliente>()), Times.Once());
        _usuarioRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Usuario>()), Times.Once());
        _emailServiceMock.Verify(x => x.EnviarEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_ExistingEmail_ShouldThrowValidationException()
    {
        var command = new CadastrarClienteCommand 
        { 
            Email = "existing@test.com",
            Nome = "Test",
            Celular = "11999999999",
            Cpf = "12345678901",
            Observacoes = "Test"
        };
        var existingCliente = Cliente.Create("Existing", "existing@test.com", "11999999999", "12345678901", "Test");

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingCliente);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("Email já cadastrado");
    }
}
