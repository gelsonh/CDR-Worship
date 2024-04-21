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

        public ScheduledSongsService(ApplicationDbContext context)
        {
            _context = context;
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



        public async Task<byte[]> GetPdfDataByIdAsync(int id, string attachmentType)
        {
            try
            {
                byte[]? attachmentData = null;

                if (attachmentType == "SongAttachment")
                {
                    var songAttachment = await _context.SongAttachments
                                                       .Where(attachment => attachment.Id == id)
                                                       .FirstOrDefaultAsync();

                    attachmentData = songAttachment?.File;
                }
                else if (attachmentType == "ChordAttachment")
                {
                    var chordAttachment = await _context.ChordAttachments
                                                        .Where(attachment => attachment.Id == id)
                                                        .FirstOrDefaultAsync();

                    attachmentData = chordAttachment?.File;
                }
                else
                {
                    throw new ArgumentException("Tipo de adjunto no válido.");
                }

                if (attachmentData != null)
                {
                    return attachmentData;
                }
                else
                {
                    throw new Exception($"No se encontró ningún documento PDF con el ID {id}.");
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                throw new Exception("Error al obtener los datos del PDF por ID.", ex);
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
            // Lógica para obtener los cantantes principales desde tu base de datos
            return await _context.Members.Where(m => m.Role == BandMembers.LeadSinger.ToString()).ToListAsync();
        }
        public async Task<List<Member>> GetBackingVocalistsAsync()
        {
            return await _context.Members.Where(m => m.Role == BandMembers.BackingVocalist.ToString()).ToListAsync();
        }

        public async Task<List<Member>> GetLeadGuitaristsAsync()
        {
            return await _context.Members.Where(m => m.Role == BandMembers.LeadGuitarist.ToString()).ToListAsync();
        }

        public async Task<List<Member>> GetSecondGuitaristsAsync()
        {
            return await _context.Members.Where(m => m.Role == BandMembers.SecondGuitarist.ToString()).ToListAsync();
        }

        public async Task<List<Member>> GetBassistsAsync()
        {
            return await _context.Members.Where(m => m.Role == BandMembers.Bassist.ToString()).ToListAsync();
        }

        public async Task<List<Member>> GetDrummersAsync()
        {
            return await _context.Members.Where(m => m.Role == BandMembers.Drummer.ToString()).ToListAsync();
        }

    }
}

