namespace CDR_Worship.Services.Interfaces
{
    public interface ISmsService
    {
        // Método para enviar el mensaje a todos los destinatarios
        void SendSms(string message);
    }
}