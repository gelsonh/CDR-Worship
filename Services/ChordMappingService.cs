using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;

namespace CDR_Worship.Services
{
    public class ChordMappingService : IChordMappingService
    {
        /// <summary>
        /// Maps a chord name to its canonical form using CDRChordMapper.
        /// </summary>
        /// <param name="chordName">The chord name to map.</param>
        /// <returns>The mapped chord name.</returns>
        /// <exception cref="ArgumentException">Thrown if the chord name is null, empty, or invalid.</exception>
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

        /// <summary>
        /// Retrieves all canonical chord names from the CDRChordMapper.
        /// </summary>
        /// <returns>A collection of all mapped chord names.</returns>
        public IEnumerable<string> GetAllMappedChordNames()
        {
            // Return distinct chord names to ensure no duplicates
            return CDRChordMapper.ChordNames.Values.Distinct();
        }
    }
}