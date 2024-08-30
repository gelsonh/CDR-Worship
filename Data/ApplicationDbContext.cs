using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CDR_Worship.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Chord> Chords { get; set; } = default!;
        public DbSet<Instrument> Instruments { get; set; } = default!;
        public DbSet<ChordAttachment> ChordAttachments { get; set; } = default!;
        public DbSet<SongAttachment> SongAttachments { get; set; } = default!;
        public DbSet<ChordDocument> ChordDocuments { get; set; } = default!;
        public DbSet<SongDocument> SongDocuments { get; set; } = default!;
        public DbSet<ScheduledSong> ScheduledSongs { get; set; } = default!;
        public DbSet<Member> Members { get; set; } = default!;
        public DbSet<DocumentComment> DocumentComments { get; set; } = default!;

    }
}
