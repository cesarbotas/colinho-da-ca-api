using ColinhoDaCa.Application.Services.Validation;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.Services.Validation;

public class CpfValidationServiceTests
{
    private readonly CpfValidationService _service;

    public CpfValidationServiceTests()
    {
        _service = new CpfValidationService();
    }

    [Theory]
    [InlineData("12345678909", true)]
    [InlineData("11144477735", true)]
    [InlineData("00000000000", false)]
    [InlineData("11111111111", false)]
    [InlineData("12345678900", false)]
    [InlineData("123", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValid_ShouldValidateCpfCorrectly(string cpf, bool expected)
    {
        var result = _service.IsValid(cpf);
        
        result.Should().Be(expected);
    }
}
