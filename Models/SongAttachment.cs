﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CDR_Worship.Models
{
    public class SongAttachment
    {

        private DateTime _created;
        public int Id { get; set; }
        public string? MusicName { get; set; }

        public string? Description { get; set; }

        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value.ToUniversalTime();
            }
        }

        public int SongDocumentId { get; set; }


        public string? AppUserId { get; set; }

        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })]
        public IFormFile? FormFile { get; set; }

        public byte[]? FileData { get; set; }

        public string? FileType { get; set; }

        // Navigation properties
        public virtual SongDocument? Song { get; set; }
        public virtual AppUser? AppUser { get; set; }
        public string? FileName { get; set; }
        public string? FileContentType { get; set; }




    }
}
