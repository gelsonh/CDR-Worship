using System.ComponentModel.DataAnnotations;
using CDR_Worship.Models;

namespace CDR_Worship.Models
{
public class ChordDocument

{
    private DateTime _created;
    private DateTime? _updated;

    public int Id { get; set; }

    [Required(ErrorMessage = "Es obligatorio")]
    public string? SongName { get; set; }

    [Required(ErrorMessage = "Es obligatorio")]
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

   // Add these properties for handling file storage
        public byte[]? FileData { get; set; }  // To store the binary data of the file

        public string? FileName { get; set; }  // To store the name of the file

        public string? FileType { get; set; }  // To store the MIME type of the file

    public int ChordId { get; set; }
    public int SongDocumentId { get; set; }

    // Navigation properties
    public virtual Chord? Chord { get; set; }
    public virtual ICollection<ChordAttachment> ChordAttachments { get; set; } = new HashSet<ChordAttachment>();
}
}