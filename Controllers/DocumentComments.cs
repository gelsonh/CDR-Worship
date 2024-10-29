using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using CDR_Worship.Models.Enums;
using CDR_Worship.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace CDR_Worship.Controllers
{
    public class DocumentCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHubContext<CommentHub> _hubContext;
        

        public DocumentCommentsController(ApplicationDbContext context, ICommentService commentService, UserManager<AppUser> userManager, IImageService imageService, IDateTimeService dateTimeService, IHubContext<CommentHub> hubContext)
        {
            _context = context;
            _commentService = commentService;
            _userManager = userManager;
            _imageService = imageService;
            _dateTimeService = dateTimeService;
            _hubContext = hubContext;
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] DocumentComment model)
    {
        if (!ModelState.IsValid)
        {
            // Retornar los errores del ModelState para depuración
            return BadRequest(ModelState);
        }

        var userId = _userManager.GetUserId(User);

        // Crear el nuevo comentario
        var comment = new DocumentComment
        {
            Comment = model.Comment,
            ScheduledSongId = model.ScheduledSongId,
            Created = DateTime.UtcNow,
            AppUserId = userId
        };

        // Obtener el nombre de la canción programada
var scheduledSongName = await _context.ScheduledSongs
    .Where(s => s.Id == model.ScheduledSongId)
    .Select(s => s.Name)
    .FirstOrDefaultAsync();

if (string.IsNullOrEmpty(scheduledSongName))
{
    Console.WriteLine($"No se encontró el nombre de la canción para ScheduledSongId: {model.ScheduledSongId}");
    return NotFound("La canción programada no se encontró.");
}

        await _commentService.AddCommentAsync(comment);

        // Obtener los datos de la imagen del usuario
        var user = await _userManager.FindByIdAsync(userId!);
        var userImage = _imageService.ConvertByteArrayToFile(user?.ImageFileData, user?.ImageFileType, DefaultImage.UserImage);

        // Preparar los datos del comentario para enviar a los clientes
        // Crear el objeto CommentData
        var commentData = new CommentData
{
    CommentId = comment.Id,
    ScheduleSongId = comment.ScheduledSongId,
    Name = scheduledSongName, // Asegurarse de que este valor esté asignado correctamente
    CommentText = comment.Comment,
    CommentUserName = user?.FirstName,
    CommentCreated = comment.Created.ToString("o"),
    CommentUserImage = userImage
};

Console.WriteLine($"CommentData Name: {commentData.Name}"); // Verificación final antes de enviar


        // Notificar a todos los clientes conectados sobre el nuevo comentario
        await _hubContext.Clients.All.SendAsync("ReceiveComment", commentData);

        // Devolver la respuesta al cliente que hizo la solicitud
        return Ok(commentData);
    }

public class CreateCommentModel
{
    [Required]
    public string? Comment { get; set; }

    [Required]
    public int ScheduledSongId { get; set; }
}

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeComment([FromBody] CommentLike model)
    {
    var userId = _userManager.GetUserId(User);

    var likeAdded = await _commentService.AddLikeAsync(model.DocumentCommentId, userId!);

    // Obtenemos el comentario actualizado para obtener el número de likes
    var comment = await _context.DocumentComments.FirstOrDefaultAsync(c => c.Id == model.DocumentCommentId);

    // Obtenemos todos los usuarios que han dado like para el tooltip
    var likedUsers = await _context.CommentLikes
        .Where(cl => cl.DocumentCommentId == model.DocumentCommentId)
        .Select(cl => cl.User!.FirstName)
        .ToListAsync();

    return Ok(new 
    {
        success = likeAdded,
        likes = comment?.LikesCount,  // Número de likes actual
        likedUsers  // Lista de nombres para el tooltip
    });
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyComment([FromBody] CommentReply model)
    {   
    if (model != null && !string.IsNullOrEmpty(model.ReplyText))
    {
        var parentComment = await _commentService.GetCommentByIdAsync(model.ParentCommentId);
        if (parentComment != null)
        {
            var reply = new DocumentComment
            {
                Comment = model.ReplyText,
                Created = DateTime.UtcNow,
                ParentCommentId = model.ParentCommentId,
                ScheduledSongId = model.ScheduledSongId,
                AppUserId = _userManager.GetUserId(User)
            };

            await _commentService.AddCommentAsync(reply);

            // Obtener los datos del usuario y la imagen
            var user = await _userManager.FindByIdAsync(reply.AppUserId!);
            var userImage = _imageService.ConvertByteArrayToFile(user?.ImageFileData, user?.ImageFileType, DefaultImage.UserImage);

            // Convertir la fecha a la hora local del servidor antes de enviarla al frontend
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(reply.Created, TimeZoneInfo.Local); // Correcto uso de la conversión a hora local

            // Formatear la fecha de creación de la respuesta usando el servicio DateTimeService
            // var formattedDate = _dateTimeService.FormatCommentDate(reply.Created);

            return Ok(new
            {
                replyId = reply.Id,
                replyText = reply.Comment,
                replyUserName = user?.FirstName,
                replyCreated = localTime.ToString("MM-dd HH:mm"),  // Usar la fecha formateada
                replyUserImage = userImage
            });
        }
    }
    return BadRequest();
}

    }
}