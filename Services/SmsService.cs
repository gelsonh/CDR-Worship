using CDR_Worship.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class SmsService : ISmsService
{
    private readonly string? _fromPhoneNumber;
    private readonly List<string> _recipients;

    public SmsService()
    {
        // Leer las credenciales de Twilio desde variables de entorno
        var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        _fromPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");

        // Inicializar Twilio
        if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(_fromPhoneNumber))
        {
            throw new InvalidOperationException("Twilio credentials are not configured properly.");
        }

        TwilioClient.Init(accountSid, authToken);

        // Obtener los destinatarios desde las variables de entorno o configurarlos aquí manualmente
        var recipientsEnv = Environment.GetEnvironmentVariable("TWILIO_RECIPIENTS");
        _recipients = recipientsEnv?.Split(',').ToList() ?? new List<string>();
    }

    // Este método envía el mensaje a todos los números de la lista de Recipients
    public void SendSms(string message)
    {
        foreach (var recipient in _recipients)
        {
            try
            {
                var messageOptions = new CreateMessageOptions(
                    new Twilio.Types.PhoneNumber(recipient.Trim()))
                {
                    From = new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                    Body = message
                };

                var msg = MessageResource.Create(messageOptions);
                Console.WriteLine($"Message sent to {recipient}: {msg.Sid}");
            }
            catch (Exception ex)
            {
                // Manejar error y registrar detalles para seguimiento
                Console.WriteLine($"Failed to send message to {recipient}: {ex.Message}");
            }
        }
    }
}