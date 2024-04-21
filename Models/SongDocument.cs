using System.ComponentModel.DataAnnotations;

namespace CDR_Worship.Models
{
    public class SongDocument
    {

        private DateTime _created;
        private DateTime? _updated;
        public int Id { get; set; }

        [Required]
        public string? SongName { get; set; }

        [Required]
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

        public byte[]? File { get; set; }

        public int ChordDocumentId { get; set; }


        // Navigation properties
     
        //public virtual ChordDocument? ChordDocument { get; set; }

        // Collections

        //public virtual ICollection<SongHistory> History { get; set; } = new HashSet<SongHistory>();q
        //public virtual ICollection<SongComment> Comments { get; set; } = new HashSet<SongComment>();
        public virtual ICollection<SongAttachment> SongAttachments { get; set; } = new HashSet<SongAttachment>();
        public virtual ICollection<ChordAttachment> ChordAttachments { get; set; } = new HashSet<ChordAttachment>();



    }
}
