using CDR_Worship.Models.Enums;

namespace CDR_Worship.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file);

        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage);
    }
}
