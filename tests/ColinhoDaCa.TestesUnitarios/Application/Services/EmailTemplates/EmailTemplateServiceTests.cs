using ColinhoDaCa.Application.Services.EmailTemplates;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.Services.EmailTemplates;

public class EmailTemplateServiceTests
{
    [Fact]
    public void GerarEmailNovaReserva_DeveRetornarHtmlValido()
    {
        var nome = "João";
        var reservaId = 1L;
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(5);
        var pets = new List<(string Nome, string Raca)> { ("Rex", "Labrador") };

        var result = EmailTemplateService.GerarEmailNovaReserva(nome, reservaId, dataInicial, dataFinal, 5, 1, 500m, 0m, 500m, "Obs", pets);

        Assert.NotNull(result);
        Assert.Contains(nome, result);
    }

    [Fact]
    public void GerarEmailReservaAlterada_DeveRetornarHtmlValido()
    {
        var nome = "João";
        var reservaId = 1L;
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(5);
        var pets = new List<(string Nome, string Raca)> { ("Rex", "Labrador") };

        var result = EmailTemplateService.GerarEmailReservaAlterada(nome, reservaId, dataInicial, dataFinal, 5, 1, 500m, 0m, 500m, "Obs", pets);

        Assert.NotNull(result);
        Assert.Contains(nome, result);
    }

    [Fact]
    public void GerarEmailContatoSite_DeveRetornarHtmlValido()
    {
        var nome = "João";
        var email = "joao@email.com";
        var assunto = "Dúvida";
        var mensagem = "Mensagem de teste";

        var result = EmailTemplateService.GerarEmailContatoSite(nome, email, assunto, mensagem);

        Assert.NotNull(result);
        Assert.Contains(nome, result);
        Assert.Contains(email, result);
        Assert.Contains(mensagem, result);
    }

    [Fact]
    public void GerarEmailConfirmacao_DeveRetornarHtmlValido()
    {
        var nome = "João";
        var reservaId = 1L;
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(5);
        var pets = new List<(string Nome, string Raca)> { ("Rex", "Labrador") };

        var result = EmailTemplateService.GerarEmailConfirmacao(nome, reservaId, dataInicial, dataFinal, 5, 1, 500m, 0m, 500m, "Obs", pets);

        Assert.NotNull(result);
        Assert.Contains(nome, result);
    }

    [Fact]
    public void GerarEmailCancelamento_DeveRetornarHtmlValido()
    {
        var nome = "João";
        var reservaId = 1L;
        var dataInicial = DateTime.Now;
        var dataFinal = DateTime.Now.AddDays(5);
        var pets = new List<(string Nome, string Raca)> { ("Rex", "Labrador") };

        var result = EmailTemplateService.GerarEmailCancelamento(nome, reservaId, dataInicial, dataFinal, 5, 1, 500m, 0m, 500m, "Obs", pets);

        Assert.NotNull(result);
        Assert.Contains(nome, result);
    }
}
