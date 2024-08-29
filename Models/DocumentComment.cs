using System.ComponentModel.DataAnnotations;

namespace CDR_Worship.Models
{
    public class DocumentComment
    {
         private DateTime _created;
        public int Id { get; set; }

        [Required]
        public string? Comment { get; set; }

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

        public int ChordDocumentId { get; set; }

        public int SongDocumentId { get; set; }

        public int ScheduledSongId { get; set; }

        public string? AppUserId { get; set; }

        // Navigation properties
        public virtual ChordDocument? ChordDocument { get; set; }

        public virtual SongDocument? SongDocument { get; set; }

        public virtual ScheduledSong? ScheduledSong { get; set; }
        
        public virtual AppUser? User { get; set; }
    }
}