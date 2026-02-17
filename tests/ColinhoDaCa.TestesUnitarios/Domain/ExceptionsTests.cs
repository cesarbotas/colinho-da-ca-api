using ColinhoDaCa.Domain._Shared.Exceptions;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class ExceptionsTests
{
    [Fact]
    public void ValidationException_DeveRetornarHttpStatusCode400()
    {
        // Arrange
        var message = "Erro de validação";

        // Act
        var exception = new ValidationException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.HttpStatusCode.Should().Be(400);
    }

    [Fact]
    public void EntityNotFoundException_ComTipoEId_DeveRetornarMensagemCorreta()
    {
        // Arrange
        var entityType = typeof(string);
        var id = 123;

        // Act
        var exception = new EntityNotFoundException(entityType, id);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.Id.Should().Be(id);
        exception.HttpStatusCode.Should().Be(404);
        exception.Message.Should().Contain("System.String");
        exception.Message.Should().Contain("123");
    }

    [Fact]
    public void EntityNotFoundException_ComMensagem_DeveRetornarMensagemPersonalizada()
    {
        // Arrange
        var message = "Entidade não encontrada";

        // Act
        var exception = new EntityNotFoundException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.HttpStatusCode.Should().Be(404);
    }

    [Fact]
    public void EntityNotFoundException_ComTipoSemId_DeveRetornarMensagemSemId()
    {
        // Arrange
        var entityType = typeof(int);

        // Act
        var exception = new EntityNotFoundException(entityType);

        // Assert
        exception.EntityType.Should().Be(entityType);
        exception.Message.Should().Contain("System.Int32");
        exception.Message.Should().NotContain("id:");
    }

    [Fact]
    public void EntityNotFoundException_ComInnerException_DevePreservarInnerException()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner error");
        var message = "Outer error";

        // Act
        var exception = new EntityNotFoundException(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }
}