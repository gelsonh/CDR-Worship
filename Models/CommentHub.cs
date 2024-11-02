using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CDR_Worship.Models
{
    public class CommentHub : Hub
    {
        // Método para enviar un comentario a todos los clientes
        public async Task SendComment(CommentData commentData)
        {
            await Clients.All.SendAsync("ReceiveComment", commentData);
        }
    }

    // Clase para estructurar los datos del comentario
    public class CommentData
    {
        public int CommentId { get; set; }
        public int ScheduleSongId { get; set; }
        public string? Name { get; set; }  // Nombre de la canción
        public string? CommentText { get; set; }
        public string? CommentUserName { get; set; }
        public string? CommentCreated { get; set; }
        public string? CommentUserImage { get; set; }
    }
}