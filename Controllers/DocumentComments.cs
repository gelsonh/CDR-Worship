using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDR_Worship.Models;
using CDR_Worship.Data;
using Microsoft.AspNetCore.Identity;

namespace CDR_Worship.Controllers
{
    public class DocumentCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DocumentCommentsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comments = await _context.DocumentComments
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
                .ToListAsync();
            return View(comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentComment documentComment)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    documentComment.AppUserId = user.Id;
                    documentComment.Created = DateTime.UtcNow;
                    _context.DocumentComments.Add(documentComment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "User not found.");
            }
            return View(documentComment);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.DocumentComments
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
                .ToListAsync();
            return PartialView("_DocumentCommentPartial", comments);
        }
    }
}
