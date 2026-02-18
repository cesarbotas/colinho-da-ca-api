using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Reservas;

public class AlterarReservaServiceTests
{
    private readonly Mock<ILogger<AlterarReservaService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IReservaRepository> _reservaRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly Mock<IRacaRepository> _racaRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AlterarReservaService _service;

    public AlterarReservaServiceTests()
    {
        _loggerMock = new Mock<ILogger<AlterarReservaService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _reservaRepositoryMock = new Mock<IReservaRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _petRepositoryMock = new Mock<IPetRepository>();
        _racaRepositoryMock = new Mock<IRacaRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _configurationMock = new Mock<IConfiguration>();
        _service = new AlterarReservaService(_loggerMock.Object, _unitOfWorkMock.Object, _reservaRepositoryMock.Object, _clienteRepositoryMock.Object, _petRepositoryMock.Object, _racaRepositoryMock.Object, _emailServiceMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_ReservaNaoEncontrada_DeveLancarExcecao()
    {
        var command = new AlterarReservaCommand();
        _reservaRepositoryMock.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Reserva, bool>>>(), It.IsAny<bool>())).ReturnsAsync((Reserva?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.Handle(1, command));
    }
}
