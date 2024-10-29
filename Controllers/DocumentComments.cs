using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using CDR_Worship.Models.Enums;
using CDR_Worship.Data;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Controllers
{
    public class DocumentCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentService _commentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IDateTimeService _dateTimeService;

        public DocumentCommentsController(ApplicationDbContext context, ICommentService commentService, UserManager<AppUser> userManager, IImageService imageService, IDateTimeService dateTimeService)
        {
            _context = context;
            _commentService = commentService;
            _userManager = userManager;
            _imageService = imageService;
            _dateTimeService = dateTimeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentComment model)
{
    if (!ModelState.IsValid)
    {
        return BadRequest();
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

    await _commentService.AddCommentAsync(comment);

 // Convertir la fecha a la hora local del servidor antes de enviarla al frontend
    var localTime = TimeZoneInfo.ConvertTimeFromUtc(comment.Created, TimeZoneInfo.Local); // Correcto uso de la conversión a hora local

    // Formatear la fecha usando el servicio
    // var formattedDate = _dateTimeService.FormatCommentDate(comment.Created);

    // Obtener los datos de la imagen del usuario
    var user = await _userManager.FindByIdAsync(userId!);
    var userImage = _imageService.ConvertByteArrayToFile(user?.ImageFileData, user?.ImageFileType, DefaultImage.UserImage);

    // Devolver la respuesta con la fecha formateada
    return Ok(new
    {
        commentId = comment.Id,
        commentText = comment.Comment,
        commentCreated = localTime.ToString("MM-dd HH:mm"),  // Usar la fecha formateada
        commentUserName = user?.FirstName,
        commentUserImage = userImage
    });
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