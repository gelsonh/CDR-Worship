using CDR_Worship.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace CDR_Worship.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
{
    try
    {
        // Verificar si el email es nulo o vacío
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("La dirección de correo es nula o vacía.");
            return; // No intentamos enviar el correo si no hay destinatario
        }

        // Obtén las configuraciones de EmailSettings
        var emailAddress = _emailSettings.EmailAddress;
        var emailPassword = _emailSettings.EmailPassword; // Este será tu SendGrid API Key
        var emailHost = _emailSettings.EmailHost;
        var emailPort = _emailSettings.EmailPort;

        var newEmail = new MimeMessage();

        // Establecer el remitente con un nombre amigable
        newEmail.Sender = new MailboxAddress("CDR Worship", emailAddress);
        newEmail.From.Add(new MailboxAddress("CDR Worship", emailAddress));

        // Agregar destinatarios
        var recipients = email.Split(";", StringSplitOptions.RemoveEmptyEntries);
        foreach (string address in recipients)
        {
            if (MailboxAddress.TryParse(address.Trim(), out var mailboxAddress))
            {
                newEmail.To.Add(mailboxAddress);
            }
            else
            {
                _logger.LogWarning($"Dirección de correo electrónico no válida: {address}");
            }
        }

        // Verificar si hay destinatarios válidos
        if (!newEmail.To.Any())
        {
            _logger.LogWarning("No se encontraron direcciones de correo electrónico válidas para enviar.");
            return; // No intentamos enviar el correo si no hay destinatarios válidos
        }

        // Establecer el asunto
        newEmail.Subject = subject;

        // Construir el cuerpo del correo electrónico
        var emailBody = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        newEmail.Body = emailBody.ToMessageBody();

        // Enviar el correo electrónico
        using var smtpClient = new SmtpClient();

        try
        {
            await smtpClient.ConnectAsync(emailHost, emailPort, SecureSocketOptions.StartTls);

            // Autenticación con SendGrid
            await smtpClient.AuthenticateAsync("apikey", emailPassword);

            await smtpClient.SendAsync(newEmail);
            await smtpClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar el correo electrónico.");
            throw;
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error en el servicio de correo electrónico.");
        throw;
    }
}
    }
}