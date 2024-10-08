﻿using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;

namespace CDR_Worship.Services
{
    public class ImageService : IImageService
    {
        private readonly string _defaultImageFilePath = "/img/Default-Image.png";

        /// <summary>
        /// Converts a byte array into a base64 image representation.
        /// </summary>
        /// <param name="fileData">The image data in byte[] format.</param>
        /// <param name="extension">The file extension of the image.</param>
        /// <param name="defaultImage">The default image type in case of null data.</param>
        /// <returns>The base64 representation of the image.</returns>
        public string ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
        {
            if (fileData == null || fileData.Length == 0)
            {
                return GetDefaultImage(defaultImage);
            }

            string imageBase64Data = Convert.ToBase64String(fileData);
            return $"data:{extension};base64,{imageBase64Data}";
        }

        /// <summary>
        /// Converts an IFormFile into a byte array.
        /// </summary>
        /// <param name="file">The file to convert.</param>
        /// <returns>The file data in byte[] format.</returns>
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
            switch (defaultImage)
            {
                case DefaultImage.UserImage:
                    return _defaultImageFilePath;
                default:
                    throw new ArgumentException("Invalid default image type.");
            }
        }
    }
}
