

namespace CDR_Worship.Models
{
public class SongAudio
{
    public int Id { get; set; }
    public string? SongName { get; set; } // Nombre de la canción
    public int? ChordId { get; set; } // Clave foránea para la tonalidad
    public virtual Chord? Chord { get; set; } // Relación con la tabla de acordes
    public string? YouTubeUrl { get; set; } // URL del video de YouTube
}
}