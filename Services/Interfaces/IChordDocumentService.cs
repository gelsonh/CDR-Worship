using CDR_Worship.Models;

namespace CDR_Worship.Services.Interfaces
{
    public interface IChordDocumentService
    {

        Task AddChordAttachmentAsync(IFormFile formFile, ChordAttachment chordAttachment, string userId);


        Task<IEnumerable<ChordDocument>> GetAllChordDocumentsAsync();

        Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId);


        Task<ChordDocument?> GetChordDocumentByIdAsync(int? chordDocumentId);

        Task<IEnumerable<Chord>> GetUniqueChordsAsync();

        Task<(byte[]? FileData, string ContentType, string FileName)?> PrepareFileForDownloadAsync(int id);

        Task<(byte[]? FileData, string ContentType)?> PrepareFileForViewAsync(int id);

        Task<(bool Success, string? ErrorMessage)> CreateChordDocumentAsync(IFormFile formFile, ChordDocument chordDocument, string userId);












        // Task<(bool Success, string? ErrorMessage)> AddChordDocumentAsync(IFormFile formFile, ChordDocument chordDocument, string? userId);

        // Task<IEnumerable<Chord>> GetUniqueChordsAsync();

        // Task<ChordDocument?> GetChordDocumentByIdAsync(int id);

        // Task<IEnumerable<ChordDocument>> GetAllChordDocumentsAsync();

        // Task<(byte[]? FileData, string? ContentType, string? FileName)?> GetAttachmentFileAsync(int id);

        // Task<ChordAttachment?> GetChordAttachmentByIdAsync(int? chordAttachmentId);


        // Task<(bool Success, string? ErrorMessage)> UpdateChordDocumentAsync(ChordDocument chordDocument);



    }
}
