using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;


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
                    return;
                }

                // Configurar SendGrid con la API Key de tus configuraciones
                var apiKey = _emailSettings.EmailPassword; // Aquí pones tu API Key de SendGrid
                var client = new SendGridClient(apiKey);

                // Crear el remitente y el destinatario
                var from = new EmailAddress(_emailSettings.EmailAddress, "CDR Worship");
                var to = new EmailAddress(email);

                // Crear el mensaje
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);

                // Enviar el correo electrónico
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Error al enviar correo electrónico: {response.StatusCode}");
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