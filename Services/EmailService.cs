using CDR_Worship.Models;
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
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("La dirección de correo es nula o vacía.");
                    return;
                }

                // Crear el mensaje de correo electrónico
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("CDR Worship", _emailSettings.EmailAddress));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage,
                    TextBody = "Por favor, usa un cliente de correo compatible con HTML para ver este mensaje."
                };
                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Enviar el correo electrónico usando el servidor SMTP de SendGrid
                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync("smtp.sendgrid.net", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // Autenticación con SendGrid usando la API Key como contraseña
                    await smtpClient.AuthenticateAsync("apikey", _emailSettings.SendGridApiKey);

                    await smtpClient.SendAsync(emailMessage);
                    await smtpClient.DisconnectAsync(true);
                }

                _logger.LogInformation("Correo enviado exitosamente a {Email}.", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de correo electrónico.");
                throw;
            }
        }
    }
}