using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Data;
using CDR_Worship.Models;

namespace CDR_Worship.Controllers
{
    public class ChordAttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChordAttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChordAttachments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChordAttachments.Include(c => c.AppUser).Include(c => c.ChordDocument);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChordAttachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chordAttachment = await _context.ChordAttachments
                .Include(c => c.AppUser)
                .Include(c => c.ChordDocument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chordAttachment == null)
            {
                return NotFound();
            }

            return View(chordAttachment);
        }

        // GET: ChordAttachments/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ChordId"] = new SelectList(_context.Chords, "Id", "Id");
            return View();
        }

        // POST: ChordAttachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MusicName,Description,Created,ChordId,AppUserId,FileData,FileType,FileName,FileContentType")] ChordAttachment chordAttachment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chordAttachment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", chordAttachment.AppUserId);
            ViewData["ChordId"] = new SelectList(_context.Chords, "Id", "Id", chordAttachment.ChordDocumentId);
            return View(chordAttachment);
        }

        // GET: ChordAttachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chordAttachment = await _context.ChordAttachments.FindAsync(id);
            if (chordAttachment == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", chordAttachment.AppUserId);
            ViewData["ChordId"] = new SelectList(_context.Chords, "Id", "Id", chordAttachment.ChordDocumentId);
            return View(chordAttachment);
        }

        // POST: ChordAttachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MusicName,Description,Created,ChordId,AppUserId,FileData,FileType,FileName,FileContentType")] ChordAttachment chordAttachment)
        {
            if (id != chordAttachment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chordAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChordAttachmentExists(chordAttachment.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", chordAttachment.AppUserId);
            ViewData["ChordId"] = new SelectList(_context.Chords, "Id", "Id", chordAttachment.ChordDocumentId);
            return View(chordAttachment);
        }

        // GET: ChordAttachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chordAttachment = await _context.ChordAttachments
                .Include(c => c.AppUser)
                .Include(c => c.ChordDocument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chordAttachment == null)
            {
                return NotFound();
            }

            return View(chordAttachment);
        }

        // POST: ChordAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chordAttachment = await _context.ChordAttachments.FindAsync(id);
            if (chordAttachment != null)
            {
                _context.ChordAttachments.Remove(chordAttachment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChordAttachmentExists(int id)
        {
            return _context.ChordAttachments.Any(e => e.Id == id);
        }
    }
}
