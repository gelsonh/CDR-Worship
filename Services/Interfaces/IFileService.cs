using CDR_Worship.Models.Enums;

namespace CDR_Worship.Services.Interfaces
{
    public interface IFileService
    {

        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        public string ConvertByteArrayToFile(byte[] fileData, string extension, DefaultImage imageType);

        public string GetFileIcon(string file);

        public string FormatFileSize(long bytes);
    }
}
