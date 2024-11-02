using CDR_Worship.Models;
using CDR_Worship.Models.ViewModel;

namespace CDR_Worship.Services.Interfaces
{
    public interface IAudioService
    {
        Task SaveAudioAsync(SongAudioViewModel model);
        Task<SongAudio?> GetSongAudioByIdAsync(int id); // Para usar en Edit/Delete
        Task<IEnumerable<SongAudio>> GetAllSongsAsync(); // Para obtener todas las canciones (Index)
        Task UpdateAudioAsync(SongAudioViewModel model); // Para editar una canción
        Task DeleteAudioAsync(int id); // Para eliminar una canción
    }
}