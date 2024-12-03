using System.Reflection.Metadata;
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
        Task<bool> AddLikeAsync(int commentId, string userId);  
        Task<bool> RemoveLikeAsync(int commentId, string userId); 

        Task ProcessCommentAsync(DocumentComment comment, string userId);
        Task ProcessRepliesAsync(IEnumerable<DocumentComment> replies, string userId);
        Task<bool> HasUserLikedCommentAsync(int commentId, string userId); 
    }
}