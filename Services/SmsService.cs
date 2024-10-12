using CDR_Worship.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class SmsService : ISmsService
{
    private readonly string? _accountSid;
    private readonly string? _authToken;
    private readonly string? _fromPhoneNumber;
    private readonly List<string> _recipients;

    public SmsService(IConfiguration configuration)
    {
        // Priorizar variables de entorno en producción, usar appsettings.json en local si no existen
        _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? configuration["Twilio:AccountSid"];
        _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN") ?? configuration["Twilio:AuthToken"];
        _fromPhoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER") ?? configuration["Twilio:PhoneNumber"];

        if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken) || string.IsNullOrEmpty(_fromPhoneNumber))
        {
            throw new InvalidOperationException("Twilio credentials are not configured properly.");
        }

        // Obtener los destinatarios de la configuración
        var recipientsEnv = Environment.GetEnvironmentVariable("TWILIO_RECIPIENTS");
        _recipients = recipientsEnv?.Split(',').ToList() ?? configuration.GetSection("Twilio:Recipients").Get<List<string>>() ?? new List<string>();

        TwilioClient.Init(_accountSid, _authToken);
    }

    public void SendSms(string message)
    {
        foreach (var recipient in _recipients)
        {
            try
            {
                var messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber(recipient.Trim()))
                {
                    From = new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                    Body = message
                };

                var msg = MessageResource.Create(messageOptions);
                Console.WriteLine($"Message sent to {recipient}: {msg.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message to {recipient}: {ex.Message}");
            }
        }
    }
}