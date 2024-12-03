using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CDR_Worship.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public CommentService(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;

        }


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

        public async Task<bool> AddLikeAsync(int documentCommentId, string userId)
        {
            var existingLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.DocumentCommentId == documentCommentId && cl.AppUserId == userId);


            if (existingLike != null) return false;

            _context.CommentLikes.Add(new CommentLike
            {
                DocumentCommentId = documentCommentId,
                AppUserId = userId,
                LikedOn = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveLikeAsync(int commentId, string userId)
        {
            var existingLike = await _context.CommentLikes
                .FirstOrDefaultAsync(cl => cl.DocumentCommentId == commentId && cl.AppUserId == userId);
            if (existingLike == null) return false;

            _context.CommentLikes.Remove(existingLike);


            await _context.SaveChangesAsync();
            return true;

        }

        public async Task ProcessCommentAsync(DocumentComment comment, string userId)
        {
            comment.FormattedDate = comment.Created.ToString("o");
            if (comment.User != null)
            {
                comment.User.ImageFilePath = _imageService.ConvertByteArrayToFile(
                    comment.User.ImageFileData,
                    comment.User.ImageFileType,
                    DefaultImage.UserImage);
            }
            comment.HasUserLiked = await _context.CommentLikes
            .AnyAsync(cl => cl.DocumentCommentId == comment.Id && cl.AppUserId == userId);
            comment.LikesCount = await _context.CommentLikes
            .CountAsync(cl => cl.DocumentCommentId == comment.Id);
        }

        public async Task ProcessRepliesAsync(IEnumerable<DocumentComment> replies, string userId)
        {
            foreach (var reply in replies)
            {
                reply.FormattedDate = reply.Created.ToString("o");
                if (reply.User != null)
                {
                    reply.User.ImageFilePath = _imageService.ConvertByteArrayToFile(
                        reply.User.ImageFileData,
                        reply.User.ImageFileType,
                        DefaultImage.UserImage
                    );
                }
                reply.HasUserLiked = await _context.CommentLikes
                .AnyAsync(cl => cl.DocumentCommentId == reply.Id && cl.AppUserId == userId);
                reply.LikesCount = await _context.CommentLikes
                .CountAsync(cl => cl.DocumentCommentId == reply.Id);
            }
        }

        public async Task<bool> HasUserLikedCommentAsync(int commentId, string userId)
        {
            return await _context.CommentLikes
                .AnyAsync(cl => cl.DocumentCommentId == commentId && cl.AppUserId == userId);
        }

       

    }
}