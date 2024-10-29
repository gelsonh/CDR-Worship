using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using CDR_Worship.Models.Enums;
using Microsoft.AspNetCore.Identity;


namespace CDR_Worship.Controllers
{
    public class ScheduledSongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private readonly IScheduledSongsService _scheduledSongService;
        private readonly ISongDocumentService _songDocumentService;
        private readonly IChordDocumentService _chordDocumentService;

        private readonly ISmsService _smsService;
        private readonly ICommentService _commentService;
        private readonly IImageService _imageService;
        private readonly IDateTimeService _dateTimeService;


        public ScheduledSongsController(ApplicationDbContext context, UserManager<AppUser> userManager, IScheduledSongsService scheduledSongService, ISongDocumentService songDocumentService, IChordDocumentService chordDocumentService, ISmsService smsService, ICommentService commentService, IImageService imageService, IDateTimeService dateTimeService)
        {
            _context = context;
            _userManager = userManager;
            _scheduledSongService = scheduledSongService;
            _songDocumentService = songDocumentService;
            _chordDocumentService = chordDocumentService;
            _smsService = smsService;
            _commentService = commentService;
            _imageService = imageService;
            _dateTimeService = dateTimeService;
            
        }


        public async Task<IActionResult> ViewPdf(int id)
        {
            try
            {
                var pdfData = await _scheduledSongService.GetAttachmentDataByIdAsync(id, "attachmentType"); // Proporciona un valor para el parámetro attachmentType
                if (pdfData != null)
                {
                    return File(pdfData, "application/pdf"); // Devuelve el PDF como un archivo para su visualización
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                ModelState.AddModelError(string.Empty, "Error al obtener los datos del PDF.");
                // Registrar la excepción
                Console.WriteLine($"An error occurred: {ex.Message}");
                return View();
            }
        }


        // GET: ScheduledSongs
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener la lista de todas las canciones programadas
                var scheduledSongs = await _scheduledSongService.GetAllScheduledSongsAsync();

                // Obtener la lista de todos los miembros
                var allMembers = await _scheduledSongService.AllMembersAsync();

                // Pasar la lista de miembros y la lista de canciones programadas a la vista
                ViewBag.Members = allMembers;


                return View(scheduledSongs);
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving scheduled songs.");
                // Registrar la excepción
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Devolver un resultado de error
                return RedirectToAction("Error");
            }
        }

        // GET: ScheduledSongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
         if (id == null)
        {
        return NotFound();
        }

    var scheduledSong = await _context.ScheduledSongs
    .Include(s => s.LeadSinger)
    .Include(s => s.Comments)
        .ThenInclude(c => c.User)
    .Include(s => s.Comments)
        .ThenInclude(c => c.Likes)
            .ThenInclude(l => l.User) // Incluir los usuarios que han dado "Me Gusta" a los comentarios
    .Include(s => s.Comments)
        .ThenInclude(c => c.Replies)
            .ThenInclude(r => r.User)
    .Include(s => s.Comments)
        .ThenInclude(c => c.Replies)
            .ThenInclude(r => r.Likes)
                .ThenInclude(l => l.User) // Incluir los usuarios que han dado "Me Gusta" a las respuestas
    .AsSplitQuery()
    .FirstOrDefaultAsync(m => m.Id == id);

    if (scheduledSong == null)
    {
        return NotFound();
    }

   // Obtener el ID del usuario actual
var userId = _userManager.GetUserId(User);

// Para cada comentario, verificar si el usuario ha dado "Me Gusta"
foreach (var comment in scheduledSong.Comments)
{
    // Mantener la fecha UTC y enviarla tal cual
    comment.FormattedDate = comment.Created.ToString("o"); // Formato ISO 8601 (UTC)

    // Procesar las imágenes de los usuarios en los comentarios
    if (comment.User != null)
    {
        comment.User.ImageFilePath = _imageService.ConvertByteArrayToFile(
            comment.User.ImageFileData!,
            comment.User.ImageFileType!,
            DefaultImage.UserImage);
    }

    // Verificar si el usuario actual ha dado "Me Gusta" a este comentario
    comment.HasUserLiked = await _context.CommentLikes
        .AnyAsync(cl => cl.DocumentCommentId == comment.Id && cl.AppUserId == userId);

    // Obtener el número total de "Me Gusta" del comentario
    comment.LikesCount = await _context.CommentLikes
        .CountAsync(cl => cl.DocumentCommentId == comment.Id);

    // Procesar las respuestas del comentario
    if (comment.Replies != null)
    {
        foreach (var reply in comment.Replies)
        {
            // Mantener la fecha UTC en las respuestas también
            reply.FormattedDate = reply.Created.ToString("o");

            // Procesar las imágenes de los usuarios en las respuestas
            if (reply.User != null)
            {
                reply.User.ImageFilePath = _imageService.ConvertByteArrayToFile(
                    reply.User.ImageFileData!,
                    reply.User.ImageFileType!,
                    DefaultImage.UserImage);
            }

            // Verificar si el usuario actual ha dado "Me Gusta" a esta respuesta
            reply.HasUserLiked = await _context.CommentLikes
                .AnyAsync(cl => cl.DocumentCommentId == reply.Id && cl.AppUserId == userId);

            // Obtener el número total de "Me Gusta" de la respuesta
            reply.LikesCount = await _context.CommentLikes
                .CountAsync(cl => cl.DocumentCommentId == reply.Id);
        }
    }
}

    return View(scheduledSong);
}
        // GET: ScheduledSongs/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var newScheduledSong = new ScheduledSong();

                // Establecer una fecha de inicio predeterminada (por ejemplo, la fecha actual)
                newScheduledSong.StartDate = DateTime.Today;

                // Obtener la lista de miembros para los diferentes roles de la banda
                var LeadSingers = await _scheduledSongService.GetLeadSingersAsync();
                var BackingVocalists = await _scheduledSongService.GetBackingVocalistsAsync();
                var BackingVocalistTwo = await _scheduledSongService.GetBackingVocalistTwoAsync();
                var LeadGuitarists = await _scheduledSongService.GetLeadGuitaristsAsync();
                var SecondGuitarists = await _scheduledSongService.GetSecondGuitaristsAsync();
                var Bassists = await _scheduledSongService.GetBassistsAsync();
                var Drummers = await _scheduledSongService.GetDrummersAsync();

                ViewBag.LeadSingers = new SelectList(LeadSingers, "Id", "MemberName");
                ViewBag.BackingVocalists = new SelectList(BackingVocalists, "Id", "MemberName");
                ViewBag.BackingVocalistTwo = new SelectList(BackingVocalistTwo, "Id", "MemberName");

                ViewBag.LeadGuitarists = new SelectList(LeadGuitarists, "Id", "MemberName");
                ViewBag.SecondGuitarists = new SelectList(SecondGuitarists, "Id", "MemberName");
                ViewBag.Bassists = new SelectList(Bassists, "Id", "MemberName");
                ViewBag.Drummers = new SelectList(Drummers, "Id", "MemberName");

                


                return View(newScheduledSong);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the scheduled song.");
                Console.WriteLine($"An error occurred: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // POST: ScheduledSongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description, StartDate, EndDate, LeadSingerId, BackingVocalistId, BackingVocalistTwoId, LeadGuitaristId, SecondGuitaristId, BassistId, DrummerId")] ScheduledSong scheduledSong)
{
    try
    {
        if (ModelState.IsValid)
        {
            // Guardar el ScheduledSong en la base de datos
            _context.Add(scheduledSong);
            await _context.SaveChangesAsync();

            // Variable para controlar el envío del mensaje, esta podría ser almacenada en una base de datos o estado persistente
bool messageSent = false; // Deberías inicializarla según tu lógica de estado (quizás en la sesión o la base de datos)

// Verificar el número de canciones programadas
var scheduledSongsCount = await _context.ScheduledSongs.CountAsync();

if (scheduledSongsCount == 3 && !messageSent)
{
    var messageLink = "https://cdr-worship-production.up.railway.app/";
    var messageBody = $"🎶 ¡El setlist está listo! 🎶 Puedes revisarlo aquí: {messageLink} 🙌 ¡Dios les bendiga y gracias por su dedicación! 🙏";
    _smsService.SendSms(messageBody);

    // Marcar que el mensaje ha sido enviado
    messageSent = true;
}
else if (scheduledSongsCount < 3)
{
    // Restablecer la bandera si el número de canciones baja de 3
    messageSent = false;
}

            return RedirectToAction("Index");
        }

        // Volver a cargar la vista con los select lists
        ViewBag.LeadSingers = new SelectList(await _scheduledSongService.GetLeadSingersAsync(), "Id", "MemberName");
        ViewBag.BackingVocalists = new SelectList(await _scheduledSongService.GetBackingVocalistsAsync(), "Id", "MemberName");
        ViewBag.BackingVocalistTwo = new SelectList(await _scheduledSongService.GetBackingVocalistTwoAsync(), "Id", "MemberName");
        ViewBag.LeadGuitarists = new SelectList(await _scheduledSongService.GetLeadGuitaristsAsync(), "Id", "MemberName");
        ViewBag.SecondGuitarists = new SelectList(await _scheduledSongService.GetSecondGuitaristsAsync(), "Id", "MemberName");
        ViewBag.Bassists = new SelectList(await _scheduledSongService.GetBassistsAsync(), "Id", "MemberName");
        ViewBag.Drummers = new SelectList(await _scheduledSongService.GetDrummersAsync(), "Id", "MemberName");

        return View(scheduledSong);
    }
    catch (Exception ex)
    {
        ModelState.AddModelError(string.Empty, $"Ocurrió un error al crear la canción programada: {ex.Message}");
        Console.WriteLine($"Error: {ex}");

        // Recargar la vista con los select lists
        ViewBag.LeadSingers = new SelectList(await _scheduledSongService.GetLeadSingersAsync(), "Id", "MemberName");
        ViewBag.BackingVocalists = new SelectList(await _scheduledSongService.GetBackingVocalistsAsync(), "Id", "MemberName");
        ViewBag.BackingVocalistTwo = new SelectList(await _scheduledSongService.GetBackingVocalistTwoAsync(), "Id", "MemberName");
        ViewBag.LeadGuitarists = new SelectList(await _scheduledSongService.GetLeadGuitaristsAsync(), "Id", "MemberName");
        ViewBag.SecondGuitarists = new SelectList(await _scheduledSongService.GetSecondGuitaristsAsync(), "Id", "MemberName");
        ViewBag.Bassists = new SelectList(await _scheduledSongService.GetBassistsAsync(), "Id", "MemberName");
        ViewBag.Drummers = new SelectList(await _scheduledSongService.GetDrummersAsync(), "Id", "MemberName");

        return View(scheduledSong);
    }
}



        // GET: ScheduledSongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledSong = await _context.ScheduledSongs.FindAsync(id);
            if (scheduledSong == null)
            {
                return NotFound();
            }

           // Obtener la lista de miembros para los diferentes roles de la banda
                var LeadSingers = await _scheduledSongService.GetLeadSingersAsync();
                var BackingVocalists = await _scheduledSongService.GetBackingVocalistsAsync();
                var BackingVocalistTwo = await _scheduledSongService.GetBackingVocalistTwoAsync();
                var LeadGuitarists = await _scheduledSongService.GetLeadGuitaristsAsync();
                var SecondGuitarists = await _scheduledSongService.GetSecondGuitaristsAsync();
                var Bassists = await _scheduledSongService.GetBassistsAsync();
                var Drummers = await _scheduledSongService.GetDrummersAsync();

                ViewBag.LeadSingers = new SelectList(LeadSingers, "Id", "MemberName");
                ViewBag.BackingVocalists = new SelectList(BackingVocalists, "Id", "MemberName");
                ViewBag.BackingVocalistTwo = new SelectList(BackingVocalistTwo, "Id", "MemberName");

                ViewBag.LeadGuitarists = new SelectList(LeadGuitarists, "Id", "MemberName");
                ViewBag.SecondGuitarists = new SelectList(SecondGuitarists, "Id", "MemberName");
                ViewBag.Bassists = new SelectList(Bassists, "Id", "MemberName");
                ViewBag.Drummers = new SelectList(Drummers, "Id", "MemberName");

            return View(scheduledSong);
        }


        // POST: ScheduledSongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SongName,Description,StartDate,EndDate,LeadSingerId,BackingVocalistId,BackingVocalistTwoId,LeadGuitaristId,SecondGuitaristId,BassistId,DrummerId")] ScheduledSong scheduledSong)        {
            if (id != scheduledSong.Id)
            {
                return NotFound();
            }

           if (ModelState.IsValid)
            {
              try
            {
               _context.Update(scheduledSong);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
            }
               catch (DbUpdateConcurrencyException)
           {
           if (!ScheduledSongExists(scheduledSong.Id))
           {
              return NotFound();
            }
            else
            {
              throw;
            }
       }
    }
      else
   {
    // Repoblar los ViewBag si el modelo no es válido
    var LeadSingers = await _scheduledSongService.GetLeadSingersAsync();
    var BackingVocalists = await _scheduledSongService.GetBackingVocalistsAsync();
    var BackingVocalistTwo = await _scheduledSongService.GetBackingVocalistTwoAsync();
    var LeadGuitarists = await _scheduledSongService.GetLeadGuitaristsAsync();
    var SecondGuitarists = await _scheduledSongService.GetSecondGuitaristsAsync();
    var Bassists = await _scheduledSongService.GetBassistsAsync();
    var Drummers = await _scheduledSongService.GetDrummersAsync();

    ViewBag.LeadSingers = new SelectList(LeadSingers, "Id", "MemberName");
    ViewBag.BackingVocalists = new SelectList(BackingVocalists, "Id", "MemberName");
    ViewBag.BackingVocalistTwo = new SelectList(BackingVocalistTwo, "Id", "MemberName");
    ViewBag.LeadGuitarists = new SelectList(LeadGuitarists, "Id", "MemberName");
    ViewBag.SecondGuitarists = new SelectList(SecondGuitarists, "Id", "MemberName");
    ViewBag.Bassists = new SelectList(Bassists, "Id", "MemberName");
    ViewBag.Drummers = new SelectList(Drummers, "Id", "MemberName");
}

return View(scheduledSong);
        }

        // GET: ScheduledSongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledSong = await _context.ScheduledSongs
                .Include(s => s.LeadSinger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduledSong == null)
            {
                return NotFound();
            }

            return View(scheduledSong);
        }

        // POST: ScheduledSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scheduledSong = await _context.ScheduledSongs.FindAsync(id);
            if (scheduledSong != null)
            {
                _context.ScheduledSongs.Remove(scheduledSong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledSongExists(int id)
        {
            return _context.ScheduledSongs.Any(e => e.Id == id);
        }
    }
}
