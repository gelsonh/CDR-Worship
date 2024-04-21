using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;

namespace CDR_Worship.Controllers
{
    public class SongAttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongAttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SongAttachments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SongAttachments.Include(s => s.AppUser).Include(s => s.Song);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SongAttachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songAttachment = await _context.SongAttachments
                .Include(s => s.AppUser)
                .Include(s => s.Song)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songAttachment == null)
            {
                return NotFound();
            }

            return View(songAttachment);
        }

        // GET: SongAttachments/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["SongId"] = new SelectList(_context.SongDocuments, "Id", "Description");
            return View();
        }

        // POST: SongAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MusicName,Description,Created,SongId,AppUserId,FileData,FileType,FileName,FileContentType")] SongAttachment songAttachment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(songAttachment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", songAttachment.AppUserId);
            ViewData["SongId"] = new SelectList(_context.SongDocuments, "Id", "Description", songAttachment.SongDocumentId);
            return View(songAttachment);
        }

        // GET: SongAttachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songAttachment = await _context.SongAttachments.FindAsync(id);
            if (songAttachment == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", songAttachment.AppUserId);
            ViewData["SongId"] = new SelectList(_context.SongDocuments, "Id", "Description", songAttachment.SongDocumentId);
            return View(songAttachment);
        }

        // POST: SongAttachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MusicName,Description,Created,SongId,AppUserId,FileData,FileType,FileName,FileContentType")] SongAttachment songAttachment)
        {
            if (id != songAttachment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongAttachmentExists(songAttachment.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", songAttachment.AppUserId);
            ViewData["SongId"] = new SelectList(_context.SongDocuments, "Id", "Description", songAttachment.SongDocumentId);
            return View(songAttachment);
        }

        // GET: SongAttachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songAttachment = await _context.SongAttachments
                .Include(s => s.AppUser)
                .Include(s => s.Song)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (songAttachment == null)
            {
                return NotFound();
            }

            return View(songAttachment);
        }

        // POST: SongAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songAttachment = await _context.SongAttachments.FindAsync(id);
            if (songAttachment != null)
            {
                _context.SongAttachments.Remove(songAttachment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongAttachmentExists(int id)
        {
            return _context.SongAttachments.Any(e => e.Id == id);
        }
    }
}
