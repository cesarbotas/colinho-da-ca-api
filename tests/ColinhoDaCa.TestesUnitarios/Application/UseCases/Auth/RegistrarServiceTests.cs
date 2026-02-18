using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Validation;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Auth;

public class RegistrarServiceTests
{
    private readonly Mock<ILogger<RegistrarService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ICpfValidationService> _cpfValidationServiceMock;
    private readonly RegistrarService _service;

    public RegistrarServiceTests()
    {
        _loggerMock = new Mock<ILogger<RegistrarService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _cpfValidationServiceMock = new Mock<ICpfValidationService>();

        _service = new RegistrarService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _usuarioRepositoryMock.Object,
            _clienteRepositoryMock.Object,
            _passwordServiceMock.Object,
            _cpfValidationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateClienteAndUsuario()
    {
        var command = new RegistrarCommand
        {
            Nome = "Test",
            Email = "test@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Senha = "password123"
        };

        _cpfValidationServiceMock.Setup(x => x.IsValid(command.Cpf)).Returns(true);
        _clienteRepositoryMock.Setup(x => x.GetByCpfAsync(command.Cpf)).ReturnsAsync((Cliente?)null);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Senha)).Returns("hashedPassword");

        await _service.Handle(command);

        _clienteRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Cliente>()), Times.Once);
        _usuarioRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ColinhoDaCa.Domain.Usuarios.Entities.Usuario>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task Handle_InvalidCpf_ShouldThrowException()
    {
        var command = new RegistrarCommand { Cpf = "invalid" };
        _cpfValidationServiceMock.Setup(x => x.IsValid(command.Cpf)).Returns(false);

        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        exception.Message.Should().Be("CPF inválido");
    }
}
