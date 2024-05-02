using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using CDR_Worship.Models.ViewModel;
using CDR_Worship.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CDR_Worship.Controllers
{
    public class ScheduledSongsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduledSongsService _scheduledSongService;
        private readonly ISongDocumentService _songDocumentService;
        private readonly IChordDocumentService _chordDocumentService;

        public ScheduledSongsController(ApplicationDbContext context, IScheduledSongsService scheduledSongService, ISongDocumentService songDocumentService, IChordDocumentService chordDocumentService)
        {
            _context = context;
            _scheduledSongService = scheduledSongService;
            _songDocumentService = songDocumentService;
            _chordDocumentService = chordDocumentService;
        }


        public async Task<IActionResult> ViewPdf(int id)
        {
            try
            {
                var pdfData = await _scheduledSongService.GetPdfDataByIdAsync(id, "attachmentType"); // Proporciona un valor para el parámetro attachmentType
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduledSong == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create([Bind("Name, Description, StartDate, EndDate, LeadSingerId, BackingVocalistId,  BackingVocalistTwoId, LeadGuitaristId, SecondGuitaristId, BassistId, DrummerId")] ScheduledSong scheduledSong)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Procesar archivos adjuntos si es necesario

                    // Guardar el ScheduledSong en la base de datos
                    _context.Add(scheduledSong);
                    await _context.SaveChangesAsync();

                    // Redirigir a la acción Index o a otra acción según sea necesario
                    return RedirectToAction("Index");
                }
                // Si el modelo no es válido, repoblar los SelectList para los roles de la banda
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
                // Manejar la excepción
                ModelState.AddModelError(string.Empty, "Ocurrió un error al crear la canción programada.");
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
                // Repoblar los SelectList para los roles de la banda antes de volver a mostrar la vista
                ViewBag.LeadSingers = new SelectList(await _scheduledSongService.GetLeadSingersAsync(), "Id", "MemberName");
                // ... repoblar los demás SelectList ...
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
            return View(scheduledSong);
        }

        // POST: ScheduledSongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Updated")] ScheduledSong scheduledSong)
        {
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
                return RedirectToAction(nameof(Index));
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
