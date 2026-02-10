using ColinhoDaCa.Application.Services.Email;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;

public class EnviarEmailService : IEnviarEmailService
{
    private readonly ILogger<EnviarEmailService> _logger;
    private readonly IEmailService _emailService;

    public EnviarEmailService(ILogger<EnviarEmailService> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Handle(EnviarEmailCommand command)
    {
        try
        {
            var corpo = $@"
                <h3>Contato do Site</h3>
                <p><strong>Nome:</strong> {command.Nome}</p>
                <p><strong>Email:</strong> {command.Email}</p>
                <p><strong>Assunto:</strong> {command.Assunto}</p>
                <p><strong>Mensagem:</strong></p>
                <p>{command.Mensagem}</p>
            ";

            await _emailService.EnviarEmailAsync(command.Assunto, corpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema ao enviar email");
            throw;
        }
    }
}