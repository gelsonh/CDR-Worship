namespace CDR_Worship.Services.Interfaces
{
    public interface IChordMappingService
    {
        /// <summary>
        /// Maps a chord name to its corresponding mapped name using CDRChordMapper.
        /// </summary>
        /// <param name="chordName">The original chord name.</param>
        /// <returns>The mapped chord name.</returns>
        /// <exception cref="ArgumentException">Thrown when the chord name is invalid.</exception>
        string MapChordName(string chordName);

        /// <summary>
        /// Retrieves all mapped chord names.
        /// </summary>
        /// <returns>An IEnumerable of mapped chord names.</returns>
        IEnumerable<string> GetAllMappedChordNames();
    }
}