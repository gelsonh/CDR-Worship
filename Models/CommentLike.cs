namespace CDR_Worship.Models
{

    public class CommentLike
    {
        public int Id { get; set; }

        public int DocumentCommentId { get; set; }  // Este es el que se usa para asociar el like con un comentario

        public string? AppUserId { get; set; }

        public DateTime LikedOn { get; set; }

        public virtual DocumentComment? DocumentComment { get; set; }

        public virtual AppUser? User { get; set; }
    }
}
