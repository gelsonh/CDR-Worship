using CDR_Worship.Models;
using CDR_Worship.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services.Interfaces
{
    public interface IScheduledSongsService
    {
        Task<List<ScheduledSong>> GetAllScheduledSongsAsync();
        Task<List<Member>> AllMembersAsync();

        Task<byte[]> GetAttachmentDataByIdAsync(int id, string attachmentType);

        public Task<ChordAttachment?> GetChordAttachmentByIdAsync(int chordAttachmentId);

        public Task<SongAttachment?> GetSongAttachmentByIdAsync(int? songAttachmentId);

        public Task<List<Member>> GetMembersByRoleAsync(BandMembers role);

        public Task<List<Member>> GetLeadSingersAsync();

        public Task<List<Member>> GetBackingVocalistsAsync();

        public Task<List<Member>> GetBackingVocalistTwoAsync();


        public Task<List<Member>> GetLeadGuitaristsAsync();

        public Task<List<Member>> GetSecondGuitaristsAsync();


        public Task<List<Member>> GetBassistsAsync();

        public Task<List<Member>> GetDrummersAsync();

    }

}
