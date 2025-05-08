using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using DiarioObras.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
        Console.WriteLine("SMTP CONFIGURADO: " + _settings.ServidorSmtp);
    }

    public async Task EnviarResetSenhaAsync(string destinatario, string link)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.NomeRemetente, _settings.Remetente));
        message.To.Add(MailboxAddress.Parse(destinatario));
        message.Subject = "Redefinição de senha - Diário de Obras";

        message.Body = new TextPart("html")
        {
            Text = $@"
        <p>Olá!</p>
        <p>Clique no botão abaixo para redefinir sua senha:</p>
        <p>
            <a href='{link}' style='
                display:inline-block;
                padding:10px 20px;
                background:#2196F3;
                color:white;
                text-decoration:none;
                border-radius:5px;
                font-weight:bold;'>
                Redefinir Senha
            </a>
        </p>
        <p>Se não foi você, ignore este e-mail.</p>"
        };


        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.ServidorSmtp, _settings.Porta, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.Remetente, _settings.Senha);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
