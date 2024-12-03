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
                throw new ArgumentException("Chord name cannot be null or empty.");
            }

            // Buscar en el diccionario el acorde con el valor directo
            var mappedChord = CDRChordMapper.ChordNames
                .FirstOrDefault(kvp => kvp.Value.Equals(chordName, StringComparison.OrdinalIgnoreCase)).Value;

            if (mappedChord != null)
            {
                return mappedChord;
            }

            throw new ArgumentException($"Invalid chord name: {chordName}");
        }

        public IEnumerable<string> GetAllMappedChordNames()
        {
            // Returns all mapped chord names from CDRChordMapper
            return CDRChordMapper.ChordNames.Values;
        }
    }
}