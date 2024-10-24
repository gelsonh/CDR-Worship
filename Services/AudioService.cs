using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Models.ViewModel;
using CDR_Worship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services
{
    public class AudioService : IAudioService
{
    private readonly ApplicationDbContext _context;

    public AudioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveAudioAsync(SongAudioViewModel model)
    {
        var songAudio = new SongAudio
        {
            SongName = model.SongName,
            YouTubeUrl = model.YouTubeUrl,
            ChordId = model.ChordId
        };

        _context.Add(songAudio);
        await _context.SaveChangesAsync();
    }

    public async Task<SongAudio?> GetSongAudioByIdAsync(int id)
    {
        return await _context.SongAudios.Include(s => s.Chord)
                                        .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<SongAudio>> GetAllSongsAsync()
    {
        return await _context.SongAudios.Include(s => s.Chord).ToListAsync();
    }

    public async Task UpdateAudioAsync(SongAudioViewModel model)
    {
        var songAudio = await _context.SongAudios.FindAsync(model.Id);
        if (songAudio != null)
        {
            songAudio.SongName = model.SongName;
            songAudio.YouTubeUrl = model.YouTubeUrl;
            songAudio.ChordId = model.ChordId;

            _context.Update(songAudio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAudioAsync(int id)
    {
        var songAudio = await _context.SongAudios.FindAsync(id);
        if (songAudio != null)
        {
            _context.SongAudios.Remove(songAudio);
            await _context.SaveChangesAsync();
        }
    }
}
}
