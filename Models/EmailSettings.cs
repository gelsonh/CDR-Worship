namespace CDR_Worship.Models
{
    public class EmailSettings
    {
        public string? EmailAddress { get; set; } // Remitente de los correos electrónicos
        public string? SendGridApiKey { get; set; } // Clave API de SendGrid
    }
}