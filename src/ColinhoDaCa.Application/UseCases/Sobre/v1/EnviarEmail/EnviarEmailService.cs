using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.Services.EmailTemplates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;

public class EnviarEmailService : IEnviarEmailService
{
    private readonly ILogger<EnviarEmailService> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public EnviarEmailService(ILogger<EnviarEmailService> logger, IEmailService emailService, IConfiguration configuration)
    {
        _logger = logger;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task Handle(EnviarEmailCommand command)
    {
        try
        {
            var corpo = EmailTemplateService.GerarEmailContatoSite(
                command.Nome,
                command.Email,
                command.Assunto,
                command.Mensagem);

            var emailDestino = _configuration["Email:EmailDestino"];
            await _emailService.EnviarEmailAsync(emailDestino, $"Contato do Site - {command.Assunto}", corpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao enviar email");
            throw;
        }
    }
}