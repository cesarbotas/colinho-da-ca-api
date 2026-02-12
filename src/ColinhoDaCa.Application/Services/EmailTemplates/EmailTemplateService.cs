namespace ColinhoDaCa.Application.Services.EmailTemplates;

public static class EmailTemplateService
{
    public static string GerarEmailCancelamento(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoes,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoes) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoes}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Reserva Cancelada",
            "hsl(0, 84%, 60%)",
            nomeCliente,
            "Informamos que sua reserva foi <strong>cancelada</strong>. Confira os detalhes abaixo:",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Se você tiver alguma dúvida ou desejar fazer uma nova reserva, entre em contato conosco."
        );
    }

    public static string GerarEmailConfirmacao(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoes,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoes) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoes}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Reserva Confirmada",
            "hsl(12, 88%, 65%)",
            nomeCliente,
            "Sua reserva foi <strong>confirmada</strong>! Por favor, envie o comprovante de pagamento para prosseguir.",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Aguardamos o envio do comprovante de pagamento para finalizar sua reserva."
        );
    }

    public static string GerarEmailPagamentoAprovado(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoes,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoes) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoes}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Pagamento Aprovado",
            "hsl(142, 71%, 45%)",
            nomeCliente,
            "Seu pagamento foi <strong>aprovado</strong> e sua reserva está <strong>confirmada</strong>!",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Aguardamos você e seus pets! Qualquer dúvida, entre em contato conosco."
        );
    }

    public static string GerarEmailNovaReserva(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoes,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoes) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoes}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Nova Reserva Criada",
            "hsl(12, 88%, 65%)",
            nomeCliente,
            "Uma nova reserva foi criada no sistema. Confira os detalhes abaixo:",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Aguarde a confirmação do cliente para prosseguir com o processo."
        );
    }

    public static string GerarEmailReservaAlterada(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoes,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoes) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoes}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Reserva Alterada",
            "hsl(45, 90%, 55%)",
            nomeCliente,
            "Uma reserva foi alterada no sistema. Confira os novos detalhes abaixo:",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Verifique as alterações realizadas pelo cliente."
        );
    }

    public static string GerarEmailComprovanteRecebido(
        string nomeCliente,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoesPagamento,
        List<(string Nome, string Raca)> pets)
    {
        var observacoesHtml = !string.IsNullOrEmpty(observacoesPagamento) ? $@"
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>📝 Observações do Pagamento:</strong><br/>
                                                    <span style='color: hsl(20, 20%, 15%);'>{observacoesPagamento}</span>
                                                </td>
                                            </tr>" : "";

        return GerarTemplateBase(
            "Comprovante Recebido",
            "hsl(200, 80%, 50%)",
            nomeCliente,
            "Um comprovante de pagamento foi enviado pelo cliente. Confira os detalhes abaixo:",
            reservaId,
            dataInicial,
            dataFinal,
            quantidadeDiarias,
            quantidadePets,
            valorTotal,
            valorDesconto,
            valorFinal,
            observacoesHtml,
            pets,
            "Verifique o comprovante e aprove o pagamento para confirmar a reserva."
        );
    }

    public static string GerarEmailContatoSite(
        string nome,
        string email,
        string assunto,
        string mensagem)
    {
        return $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Contato do Site</title>
</head>
<body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, sans-serif; background-color: hsl(25, 30%, 98%);'>
    <table width='100%' cellpadding='0' cellspacing='0' style='background-color: hsl(25, 30%, 98%); padding: 20px;'>
        <tr>
            <td align='center'>
                <table width='600' cellpadding='0' cellspacing='0' style='background-color: hsl(0, 0%, 100%); border-radius: 1rem; overflow: hidden; box-shadow: 0 4px 20px -4px hsla(12, 88%, 65%, 0.15);'>
                    <tr>
                        <td style='background: linear-gradient(135deg, hsl(12, 88%, 65%) 0%, hsl(340, 82%, 75%) 100%); padding: 40px 30px; text-align: center;'>
                            <h1 style='color: hsl(0, 0%, 100%); margin: 0; font-size: 28px; font-weight: bold;'>🐾 Colinho da Cá</h1>
                            <p style='color: hsl(0, 0%, 100%); margin: 10px 0 0 0; font-size: 16px; opacity: 0.95;'>Hotel para Pets</p>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 40px 30px;'>
                            <h2 style='color: hsl(12, 88%, 65%); margin: 0 0 20px 0; font-size: 24px;'>Contato do Site</h2>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0 0 30px 0;'>
                                Você recebeu uma nova mensagem através do formulário de contato do site.
                            </p>
                            <table width='100%' cellpadding='0' cellspacing='0' style='background-color: hsl(25, 20%, 92%); border-radius: 0.75rem; border: 1px solid hsl(25, 15%, 88%); margin-bottom: 30px;'>
                                <tr>
                                    <td style='padding: 25px;'>
                                        <table width='100%' cellpadding='8' cellspacing='0'>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 8px 0;'>
                                                    <strong>👤 Nome:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 8px 0;'>
                                                    {nome}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 8px 0;'>
                                                    <strong>📧 Email:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 8px 0;'>
                                                    {email}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 8px 0;'>
                                                    <strong>📋 Assunto:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 8px 0;'>
                                                    {assunto}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 12px 0;'>
                                                    <strong>💬 Mensagem:</strong><br/>
                                                    <div style='color: hsl(20, 20%, 15%); margin-top: 10px; padding: 15px; background-color: hsl(0, 0%, 100%); border-radius: 0.5rem; line-height: 1.6;'>
                                                        {mensagem}
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0;'>
                                Responda este email para entrar em contato com o cliente.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style='background-color: hsl(25, 20%, 92%); padding: 30px; text-align: center; border-top: 1px solid hsl(25, 15%, 88%);'>
                            <p style='color: hsl(20, 10%, 45%); font-size: 14px; margin: 0 0 10px 0;'>
                                📧 contato@colinhodaca.com.br | 📱 (11) 99999-9999
                            </p>
                            <p style='color: hsl(20, 10%, 60%); font-size: 12px; margin: 0;'>
                                © 2024 Colinho da Cá - Hotel para Pets. Todos os direitos reservados.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }

    private static string GerarTemplateBase(
        string titulo,
        string corTitulo,
        string nomeCliente,
        string mensagemPrincipal,
        long reservaId,
        DateTime dataInicial,
        DateTime dataFinal,
        int quantidadeDiarias,
        int quantidadePets,
        decimal valorTotal,
        decimal valorDesconto,
        decimal valorFinal,
        string observacoesHtml,
        List<(string Nome, string Raca)> pets,
        string mensagemFinal)
    {
        return $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{titulo}</title>
</head>
<body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, sans-serif; background-color: hsl(25, 30%, 98%);'>
    <table width='100%' cellpadding='0' cellspacing='0' style='background-color: hsl(25, 30%, 98%); padding: 20px;'>
        <tr>
            <td align='center'>
                <table width='600' cellpadding='0' cellspacing='0' style='background-color: hsl(0, 0%, 100%); border-radius: 1rem; overflow: hidden; box-shadow: 0 4px 20px -4px hsla(12, 88%, 65%, 0.15);'>
                    <tr>
                        <td style='background: linear-gradient(135deg, hsl(12, 88%, 65%) 0%, hsl(340, 82%, 75%) 100%); padding: 40px 30px; text-align: center;'>
                            <h1 style='color: hsl(0, 0%, 100%); margin: 0; font-size: 28px; font-weight: bold;'>🐾 Colinho da Cá</h1>
                            <p style='color: hsl(0, 0%, 100%); margin: 10px 0 0 0; font-size: 16px; opacity: 0.95;'>Hotel para Pets</p>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 40px 30px;'>
                            <h2 style='color: {corTitulo}; margin: 0 0 20px 0; font-size: 24px;'>{titulo}</h2>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;'>
                                Olá <strong>{nomeCliente}</strong>,
                            </p>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0 0 30px 0;'>
                                {mensagemPrincipal}
                            </p>
                            <table width='100%' cellpadding='0' cellspacing='0' style='background-color: hsl(25, 20%, 92%); border-radius: 0.75rem; border: 1px solid hsl(25, 15%, 88%); margin-bottom: 30px;'>
                                <tr>
                                    <td style='padding: 25px;'>
                                        <table width='100%' cellpadding='8' cellspacing='0'>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 8px 0;'>
                                                    <strong>Número da Reserva:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 8px 0;'>
                                                    #{reservaId.ToString().PadLeft(6, '0')}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>📅 Check-in:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    {dataInicial:dd/MM/yyyy HH:mm}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>📅 Check-out:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    {dataFinal:dd/MM/yyyy HH:mm}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>🌙 Diárias:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    {quantidadeDiarias}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>🐕 Pets:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    {quantidadePets}
                                                </td>
                                            </tr>
                                            {(pets != null && pets.Count > 0 ? $@"<tr>
                                                <td colspan='2' style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0 0 0;'>
                                                    <strong>🐾 Pets da Reserva:</strong>
                                                </td>
                                            </tr>
                                            {string.Join("", pets.Select(p => $@"<tr>
                                                <td colspan='2' style='color: hsl(20, 20%, 15%); font-size: 14px; padding: 2px 0 2px 15px;'>
                                                    {p.Nome} - {p.Raca}
                                                </td>
                                            </tr>"))}" : "")}
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0; padding-top: 6px;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>💰 Subtotal:</strong>
                                                </td>
                                                <td style='color: hsl(20, 20%, 15%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    R$ {valorTotal:N2}
                                                </td>
                                            </tr>
                                            {(valorDesconto > 0 ? $@"<tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 14px; padding: 6px 0;'>
                                                    <strong>🎟️ Desconto:</strong>
                                                </td>
                                                <td style='color: hsl(142, 71%, 45%); font-size: 14px; text-align: right; padding: 6px 0;'>
                                                    - R$ {valorDesconto:N2}
                                                </td>
                                            </tr>" : "")}
                                            <tr>
                                                <td colspan='2' style='border-top: 1px solid hsl(25, 15%, 88%); padding: 0;'></td>
                                            </tr>
                                            <tr>
                                                <td style='color: hsl(20, 10%, 45%); font-size: 16px; padding: 8px 0;'>
                                                    <strong>💰 Valor Final:</strong>
                                                </td>
                                                <td style='color: hsl(12, 88%, 65%); font-size: 18px; font-weight: bold; text-align: right; padding: 8px 0;'>
                                                    R$ {valorFinal:N2}
                                                </td>
                                            </tr>
                                            {observacoesHtml}
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0 0 15px 0;'>
                                {mensagemFinal}
                            </p>
                            <p style='color: hsl(20, 20%, 15%); font-size: 16px; line-height: 1.6; margin: 0;'>
                                Atenciosamente,<br/>
                                <strong>Equipe Colinho da Cá</strong> 🐾
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style='background-color: hsl(25, 20%, 92%); padding: 30px; text-align: center; border-top: 1px solid hsl(25, 15%, 88%);'>
                            <p style='color: hsl(20, 10%, 45%); font-size: 14px; margin: 0 0 10px 0;'>
                                📧 contato@colinhodaca.com.br | 📱 (11) 99999-9999
                            </p>
                            <p style='color: hsl(20, 10%, 60%); font-size: 12px; margin: 0;'>
                                © 2024 Colinho da Cá - Hotel para Pets. Todos os direitos reservados.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }
}
