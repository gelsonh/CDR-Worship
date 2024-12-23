﻿using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;

namespace CDR_Worship.Services
{
    public class FileService : IFileService
    {
        #region Properties
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        private readonly string _defaultAppUserImageSrc = "/img/DefaultUserImage.png";
       
        #endregion

        #region Convert Byte Array to File
        public string ConvertByteArrayToFile(byte[] fileData, string extension, DefaultImage fileType)
        {
            if ((fileData == null || fileData.Length == 0))
            {
                switch (fileType)
                {
                    case DefaultImage.UserImage: return _defaultAppUserImageSrc;
                }
            }

            try
            {
                string fileBase64Data = Convert.ToBase64String(fileData!);
                return string.Format($"data:{extension};base64,{fileBase64Data}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Convert File to Byte Array
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();
                memoryStream.Dispose();

                return byteFile;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Format File Size
        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal fileSize = bytes;
            while (Math.Round(fileSize / 1024) >= 1)
            {
                fileSize /= bytes;
                counter++;
            }
            return string.Format("{0:n1}{1}", fileSize, suffixes[counter]);

        }

        #endregion

        #region Get File Icon
        public string GetFileIcon(string file)
        {
            string fileImage = "default";

            if (!string.IsNullOrWhiteSpace(file))
            {
                fileImage = Path.GetExtension(file).Replace(".", "");
                return $"/img/content-type/png/{fileImage}.png";
            }
            return fileImage;
        }

        #endregion    
    }
}
