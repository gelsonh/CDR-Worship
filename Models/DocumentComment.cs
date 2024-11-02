using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        // public int Likes { get; set; } // Number of likes

        [NotMapped] // Esta propiedad no se mapea a una columna de la base de datos
        public int LikesCount { get; set; }

        [NotMapped] // Indica que esta propiedad no se mapea a una columna de la base de datos
        public bool HasUserLiked { get; set; }

        public string? FormattedDate { get; set; }
        public int? ParentCommentId { get; set; } // Reference to the parent comment if it's a reply
        public virtual DocumentComment? ParentComment { get; set; }
        public virtual ICollection<DocumentComment> Replies { get; set; } = new List<DocumentComment>();
        // Aquí, 'Likes' debe ser una colección y no debe tener [NotMapped]
        public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();

        public string? AppUserId { get; set; }
        public virtual AppUser? User { get; set; }

        public int ScheduledSongId { get; set; }
        public virtual ScheduledSong? ScheduledSong { get; set; }
    }
}