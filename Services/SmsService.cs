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
        _accountSid = configuration["Twilio:AccountSid"];
        _authToken = configuration["Twilio:AuthToken"];
        _fromPhoneNumber = configuration["Twilio:PhoneNumber"];
        
        // Obtener los destinatarios de la configuración
        _recipients = configuration.GetSection("Twilio:Recipients").Get<List<string>>() ?? new List<string>();

        // Inicializar el cliente de Twilio
        TwilioClient.Init(_accountSid, _authToken);
    }

    // Este método envía el mensaje a todos los números de la lista de Recipients
    public void SendSms(string message)
    {
        foreach (var recipient in _recipients)
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
    }

   
}