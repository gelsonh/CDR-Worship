using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

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

                // Verificar si la API Key de SendGrid está disponible
                var apiKey = _emailSettings.SendGridApiKey;
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("La clave API de SendGrid es nula o vacía.");
                    throw new ArgumentNullException(nameof(apiKey), "La clave API no puede ser nula.");
                }

                // Configurar el cliente de SendGrid
                var client = new SendGridClient(apiKey);

                // Crear el remitente y destinatario
                var from = new EmailAddress(_emailSettings.EmailAddress, "CDR Worship");
                var to = new EmailAddress(email);

                // Crear el mensaje
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);

                // Enviar el correo electrónico
                var response = await client.SendEmailAsync(msg);

                // Verificar el estado de la respuesta
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError($"Error al enviar correo electrónico. Código de estado: {response.StatusCode}, Respuesta: {responseBody}");
                }
                else
                {
                    _logger.LogInformation("Correo enviado exitosamente.");
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