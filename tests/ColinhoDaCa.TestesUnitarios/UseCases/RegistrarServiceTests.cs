using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Validation;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class RegistrarServiceTests
{
    private readonly Mock<ILogger<RegistrarService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ICpfValidationService> _cpfValidationMock;
    private readonly RegistrarService _service;

    public RegistrarServiceTests()
    {
        _loggerMock = new Mock<ILogger<RegistrarService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _cpfValidationMock = new Mock<ICpfValidationService>();

        _service = new RegistrarService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _clienteRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _passwordServiceMock.Object,
            _cpfValidationMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateClienteAndUsuario()
    {
        // Arrange
        var command = new RegistrarCommand
        {
            Nome = "João Silva",
            Email = "joao@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Senha = "MinhaSenh@123"
        };

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((Cliente?)null);
        _clienteRepositoryMock.Setup(x => x.GetByCpfAsync(command.Cpf))
            .ReturnsAsync((Cliente?)null);
        _cpfValidationMock.Setup(x => x.IsValid(command.Cpf))
            .Returns(true);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Senha))
            .Returns("hashedPassword");

        // Act
        await _service.Handle(command);

        // Assert
        _clienteRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Cliente>()), Times.Once());
        _usuarioRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Usuario>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.AtLeast(1));
    }

    [Fact]
    public async Task Handle_ExistingEmail_ShouldThrowValidationException()
    {
        // Arrange
        var command = new RegistrarCommand { Email = "existing@test.com" };
        var existingCliente = Cliente.Create("Existing", "existing@test.com", "11999999999", "12345678901", "Test");

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(existingCliente);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("Email já cadastrado");
    }

    [Fact]
    public async Task Handle_InvalidCpf_ShouldThrowValidationException()
    {
        // Arrange
        var command = new RegistrarCommand 
        { 
            Email = "new@test.com",
            Cpf = "invalidcpf"
        };

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((Cliente?)null);
        _cpfValidationMock.Setup(x => x.IsValid(command.Cpf))
            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("CPF inválido");
    }
}