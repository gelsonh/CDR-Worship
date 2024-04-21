using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CDR_Worship.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CDR_Worship.Models.Chord> Chords { get; set; } = default!;
        public DbSet<CDR_Worship.Models.Instrument> Instruments { get; set; } = default!;
        public DbSet<CDR_Worship.Models.ChordAttachment> ChordAttachments { get; set; } = default!;
        public DbSet<CDR_Worship.Models.SongAttachment> SongAttachments { get; set; } = default!;
        public DbSet<CDR_Worship.Models.ChordDocument> ChordDocuments { get; set; } = default!;
        public DbSet<CDR_Worship.Models.SongDocument> SongDocuments { get; set; } = default!;
        public DbSet<CDR_Worship.Models.ScheduledSong> ScheduledSongs { get; set; } = default!;
        public DbSet<CDR_Worship.Models.Member> Members { get; set; } = default!;
    }
}
