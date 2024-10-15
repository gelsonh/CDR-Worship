using System.ComponentModel.DataAnnotations;

namespace CDR_Worship.Models
{
    public class SongDocument
    {

        private DateTime _created;
        private DateTime? _updated;
        public int Id { get; set; }

        [Required(ErrorMessage = "Es obligatorio")]
        public string? SongName { get; set; }

        [Required(ErrorMessage = "Es obligatorio")]
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

        public DateTime? Updated
        {
            get => _updated;

            set
            {
                if (value.HasValue)
                {
                    _updated = value.Value.ToUniversalTime();
                }
                else
                {
                    _updated = null;
                }
            }
        }

        public byte[]? FileData { get; set; }  // To store the binary data of the file

        public string? FileName { get; set; }  // To store the name of the file

        public string? FileType { get; set; }  // To store the MIME type of the file


        public int ChordDocumentId { get; set; }


        public virtual ICollection<SongAttachment> SongAttachments { get; set; } = new HashSet<SongAttachment>();
       

    }
}
