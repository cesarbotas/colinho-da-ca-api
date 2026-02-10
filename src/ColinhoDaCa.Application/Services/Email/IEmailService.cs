namespace ColinhoDaCa.Application.Services.Email;

public interface IEmailService
{
    Task EnviarEmailAsync(string assunto, string corpo);
}
