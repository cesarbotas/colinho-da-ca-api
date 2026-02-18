using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Reservas;

public class CadastrarReservaServiceTests
{
    private readonly Mock<ILogger<CadastrarReservaService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IReservaRepository> _reservaRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly Mock<IRacaRepository> _racaRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly CadastrarReservaService _service;

    public CadastrarReservaServiceTests()
    {
        _loggerMock = new Mock<ILogger<CadastrarReservaService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _reservaRepositoryMock = new Mock<IReservaRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _petRepositoryMock = new Mock<IPetRepository>();
        _racaRepositoryMock = new Mock<IRacaRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _configurationMock = new Mock<IConfiguration>();

        _service = new CadastrarReservaService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _reservaRepositoryMock.Object,
            _clienteRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _petRepositoryMock.Object,
            _racaRepositoryMock.Object,
            _emailServiceMock.Object,
            _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateReserva()
    {
        var command = new CadastrarReservaCommand
        {
            ClienteId = 1,
            DataInicial = DateTime.Now,
            DataFinal = DateTime.Now.AddDays(5),
            QuantidadeDiarias = 5,
            QuantidadePets = 1,
            ValorTotal = 500,
            ValorFinal = 500,
            Observacoes = "Teste",
            PetIds = new List<long> { 1 },
            UsuarioId = 1
        };

        await _service.Handle(command);

        _reservaRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Reserva>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
