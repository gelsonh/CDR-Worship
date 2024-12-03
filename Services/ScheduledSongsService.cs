using CDR_Worship.Data;
using CDR_Worship.Models;
using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Worship.Services
{
    public class ScheduledSongsService : IScheduledSongsService
    {

        private readonly ApplicationDbContext _context;
        private readonly ICommentService _commentService;

        public ScheduledSongsService(ApplicationDbContext context, ICommentService commentService)
        {
            _context = context;
            _commentService = commentService;
        }

        public async Task<List<ScheduledSong>> GetAllScheduledSongsAsync()
        {
            return await _context.ScheduledSongs

                                .ToListAsync();
        }

        public async Task<List<Member>> AllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }


        public async Task<byte[]> GetAttachmentDataByIdAsync(int id, string attachmentType)
        {
            try
            {
                byte[]? attachmentData = null;

                if (attachmentType == "SongAttachment")
                {
                    var songAttachment = await _context.SongAttachments
                                                       .FirstOrDefaultAsync(attachment => attachment.Id == id);

                    attachmentData = songAttachment?.FileData;  // Obtener los datos del archivo
                }
                else if (attachmentType == "ChordAttachment")
                {
                    var chordAttachment = await _context.ChordAttachments
                                                        .FirstOrDefaultAsync(attachment => attachment.Id == id);

                    attachmentData = chordAttachment?.FileData;  // Obtener los datos del archivo
                }
                else
                {
                    throw new ArgumentException("Tipo de adjunto no válido.");
                }

                if (attachmentData != null && attachmentData.Length > 0)
                {
                    return attachmentData;
                }
                else
                {
                    throw new Exception($"No se encontró ningún documento con el ID {id}.");
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                throw new Exception("Error al obtener los datos del archivo por ID.", ex);
            }
        }
        public async Task<ChordAttachment?> GetChordAttachmentByIdAsync(int chordAttachmentId)
        {

            try
            {
                ChordAttachment? chordAttachment = await _context.ChordAttachments
                    .Include(ca => ca.ChordDocument) // Incluir datos del documento de acorde asociado
                    .Include(ca => ca.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == chordAttachmentId);

                return chordAttachment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting ChordAttachment", ex);
            }


        }

        public async Task<SongAttachment?> GetSongAttachmentByIdAsync(int? songAttachmentId)
        {
            try
            {
                SongAttachment? songAttachment = await _context.SongAttachments
                    .Include(t => t.Song)
                    .Include(t => t.AppUser)
                    .FirstOrDefaultAsync(c => c.Id == songAttachmentId);

                return songAttachment;
            }

            catch (Exception ex)
            {
                throw new Exception("Error getting ChordAttachment", ex);
            }
        }

        public async Task<List<Member>> GetMembersByRoleAsync(BandMembers role)
        {
            try
            {
                return await _context.Members
                    .Where(m => m.Role == role.ToString())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                throw new Exception($"Error al obtener miembros del rol {role}.", ex);
            }
        }

        public async Task<List<Member>> GetLeadSingersAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.LeadSinger.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<List<Member>> GetBackingVocalistsAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.BackingVocalist.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }


        public async Task<List<Member>> GetBackingVocalistTwoAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.BackingVocalistTwo.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<List<Member>> GetLeadGuitaristsAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.LeadGuitarist.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<List<Member>> GetSecondGuitaristsAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.SecondGuitarist.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<List<Member>> GetBassistsAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.Bassist.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<List<Member>> GetDrummersAsync()
        {
            return await _context.Members
                                 .Where(m => m.Role == BandMembers.Drummer.ToString())
                                 .GroupBy(m => m.MemberName)
                                 .Select(group => group.First())
                                 .ToListAsync();
        }

        public async Task<ScheduledSong?> GetScheduledSongDetailsAsync(int id, string userId)
        {
            var scheduledSong = await _context.ScheduledSongs
            .Include(s => s.LeadSinger)
            .Include(s => s.Comments).ThenInclude(c => c.User)
            .Include(s => s.Comments).ThenInclude(c => c.Likes)
            .Include(s => s.Comments).ThenInclude(c => c.Replies).ThenInclude(r => r.User)
            .Include(s => s.Comments).ThenInclude(c => c.Replies).ThenInclude(r => r.Likes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduledSong == null) return null;

            foreach (var comment in scheduledSong.Comments)
            {
                await _commentService.ProcessCommentAsync(comment, userId);
                if (comment.Replies != null)
                {
                    await  _commentService.ProcessRepliesAsync(comment.Replies, userId);
                }

            }
            return scheduledSong;
        }

    }
}

