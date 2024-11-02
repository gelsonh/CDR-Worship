using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;

public class ImageService : IImageService
{
    // Solo incluye la ruta relativa dentro de wwwroot
    private readonly string _defaultImageFilePath = "img/DefaultImage.png";

    public string ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
    {
        if (fileData == null || fileData.Length == 0)
        {
            return GetDefaultImage(defaultImage);
        }

        // Convertir la extensión a un tipo MIME
        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentException("File extension is null or empty.");
        }
        var mimeType = GetMimeType(extension);

        string imageBase64Data = Convert.ToBase64String(fileData);
        return $"data:{mimeType};base64,{imageBase64Data}";
    }

    public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is null or empty.");
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    private string GetDefaultImage(DefaultImage defaultImage)
    {
        // Construir la ruta completa usando Path.Combine
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", _defaultImageFilePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Default image file not found.");
        }

        // Leer el archivo de imagen y convertirlo en una cadena base64
        var fileData = File.ReadAllBytes(filePath);
        var imageBase64Data = Convert.ToBase64String(fileData);
        return $"data:image/png;base64,{imageBase64Data}"; // Suponiendo que la imagen predeterminada es PNG
    }

    private string GetMimeType(string extension)
    {
        return extension.ToLower() switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => throw new ArgumentException("Unsupported image format"),
        };
    }
}