using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ColinhoDaCa.Application.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, 
        ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task EnviarEmailAsync(string destinatario, string assunto, string corpo)
    {
        try
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = _configuration["Email:SmtpPort"];
            var smtpUser = _configuration["Email:SmtpUser"];
            var smtpPassword = _configuration["Email:SmtpPassword"];

            // Se as configurações de email não estiverem definidas, apenas loga e retorna
            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("Configurações de email não definidas. Email não enviado para: {Destinatario}, Assunto: {Assunto}", destinatario, assunto);
                return;
            }

            var remetenteEmail = _configuration["Email:RemetenteEmail"];
            var remetenteNome = _configuration["Email:RemetenteNome"];

            using var client = new SmtpClient(smtpHost, int.Parse(smtpPort))
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(remetenteEmail, remetenteNome),
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = false,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            mailMessage.To.Add(destinatario);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email enviado com sucesso para: {Destinatario}", destinatario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para: {Destinatario}. Assunto: {Assunto}", destinatario, assunto);
            // Não relança a exceção para não quebrar o fluxo principal
        }
    }
}
