using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CDR_Worship.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Existing methods

        public async Task AddCommentAsync(DocumentComment comment)
        {
            comment.Created = DateTime.UtcNow;
            _context.DocumentComments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DocumentComment>> GetCommentsByScheduledSongIdAsync(int scheduledSongId)
        {
            return await _context.DocumentComments
                .Where(c => c.ScheduledSongId == scheduledSongId)
                .Include(c => c.User)
                .Include(c => c.Replies).ThenInclude(r => r.User)
                .OrderByDescending(c => c.Created)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<DocumentComment?> GetCommentByIdAsync(int id)
        {
            return await _context.DocumentComments
                .Include(c => c.User)
                .Include(c => c.Replies).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCommentAsync(DocumentComment comment)
        {
            _context.DocumentComments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.DocumentComments.FindAsync(id);
            if (comment != null)
            {
                _context.DocumentComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        // New Like Methods

        // Add a like to a comment, and ensure a user can like only once.
        public async Task<bool> AddLikeAsync(int documentCommentId, string userId)
        {
            var existingLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.DocumentCommentId == documentCommentId && cl.AppUserId == userId);

            if (existingLike != null)
            {
                // El usuario ya ha dado like a este comentario
                return false;
            }

            var comment = await _context.DocumentComments.FindAsync(documentCommentId);
            if (comment != null)
            {
                comment.LikesCount += 1;
                _context.CommentLikes.Add(new CommentLike
                {
                    DocumentCommentId = documentCommentId,
                    AppUserId = userId,
                    LikedOn = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // Remove a like if needed (optional functionality)
        public async Task<bool> RemoveLikeAsync(int commentId, string userId)
        {
            var existingLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.DocumentCommentId == commentId && cl.AppUserId == userId);

            if (existingLike != null)
            {
                _context.CommentLikes.Remove(existingLike);

                var comment = await _context.DocumentComments.FindAsync(commentId);
                if (comment != null)
                {
                    comment.LikesCount -= 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        // Check if the user has already liked a comment
        public async Task<bool> HasUserLikedCommentAsync(int commentId, string userId)
        {
            return await _context.CommentLikes
                .AnyAsync(cl => cl.DocumentCommentId == commentId && cl.AppUserId == userId);
        }


    }
}