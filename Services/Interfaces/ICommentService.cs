using CDR_Worship.Models;

namespace CDR_Worship.Services.Interfaces
{
    public interface ICommentService
    {
        // Comment-related methods
        Task AddCommentAsync(DocumentComment comment);
        Task<IEnumerable<DocumentComment>> GetCommentsByScheduledSongIdAsync(int scheduledSongId);
        Task<DocumentComment?> GetCommentByIdAsync(int id);
        Task UpdateCommentAsync(DocumentComment comment);
        Task DeleteCommentAsync(int id);

        // Like-related methods
        Task<bool> AddLikeAsync(int commentId, string userId);  // Add a like to a comment
        Task<bool> RemoveLikeAsync(int commentId, string userId);  // Optional: remove a like
        Task<bool> HasUserLikedCommentAsync(int commentId, string userId);  // Check if user has already liked the comment
    }
}