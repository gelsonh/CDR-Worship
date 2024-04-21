using CDR_Worship.Models;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services.Interfaces
{
    public interface ISongDocumentService
    {

     
        public Task AddSongAttachmentAsync(SongAttachment? SongAttachment);

        public Task<SongAttachment?> GetSongAttachmentByIdAsync(int? songAttachmentId);

        public Task<SongDocument?> GetSongDocumentByIdAsync(int? songDocumentId);
        public Task<IEnumerable<SongDocument>> GetAllSongDocumentsAsync();
        
    }
}
