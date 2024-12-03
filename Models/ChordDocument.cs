using System.ComponentModel.DataAnnotations;

namespace CDR_Worship.Models
{
    public class ChordDocument
    {
        private DateTime _created;
        private DateTime? _updated;

        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la canción es obligatorio.")]
        public string? SongName { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Description { get; set; }

        public string? Tempo { get; set; }

        public DateTime Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        public DateTime? Updated
        {
            get => _updated;
            set => _updated = value?.ToUniversalTime();
        }

        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un acorde.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un acorde válido.")]
        public int ChordId { get; set; }

        public int SongDocumentId { get; set; }

        public virtual Chord? Chord { get; set; }
        public virtual ICollection<ChordAttachment> ChordAttachments { get; set; } = new HashSet<ChordAttachment>();
    }
}