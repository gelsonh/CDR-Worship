using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;

namespace CDR_Worship.Services
{
    public class ChordMappingService : IChordMappingService
    {
       
        public string MapChordName(string chordName)
        {
            if (string.IsNullOrWhiteSpace(chordName))
            {
                throw new ArgumentException("Chord name cannot be null or empty.", nameof(chordName));
            }

            // Attempt to find the chord name in the dictionary
            var mappedChord = CDRChordMapper.ChordNames
                .FirstOrDefault(kvp => kvp.Value.Equals(chordName, StringComparison.OrdinalIgnoreCase)).Value;

            if (!string.IsNullOrEmpty(mappedChord))
            {
                return mappedChord;
            }

            throw new ArgumentException($"Invalid chord name: {chordName}", nameof(chordName));
        }

        public IEnumerable<string> GetAllMappedChordNames()
        {
            // Return distinct chord names to ensure no duplicates
            return CDRChordMapper.ChordNames.Values.Distinct();
        }
    }
}