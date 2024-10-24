namespace CDR_Worship.Models.ViewModel
{
    public class SongAudioViewModel
    {
        public int Id { get; set; }
        public string? SongName { get; set; }
        public string? YouTubeUrl { get; set; }
        public int? ChordId { get; set; }
        public IEnumerable<Chord>? Chords { get; set; }
    }
}