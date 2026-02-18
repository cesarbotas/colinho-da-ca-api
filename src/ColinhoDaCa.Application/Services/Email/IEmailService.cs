namespace ColinhoDaCa.Application.Services.Email;

public interface IEmailService
{
    Task EnviarEmailAsync(string destinatario, string assunto, string corpo);
}
