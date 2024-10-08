namespace CDR_Worship.Models.Enums
{
    public enum CDRChord
    {
        C,
        Cm,
        Csos,   // C#
        Csosm,  // C#m
        D,
        Dm,
        Dsos,   // D#
        Dsosm,  // D#m
        E,
        Em,
        F,
        Fm,
        Fsos,   // F#
        Fsosm,  // F#m
        G,
        Gm,
        Gsos,   // G#
        Gsosm,  // G#m
        A,
        Am,
        Asos,   // A#
        Asosm,  // A#m
        B,
        Bm
    }

    public static class CDRChordMapper
    {
        public static readonly Dictionary<CDRChord, string> ChordNames = new()
        {
            { CDRChord.C, "C" },
            { CDRChord.Cm, "Cm" },
            { CDRChord.Csos, "C#" },
            { CDRChord.Csosm, "C#m" },
            { CDRChord.D, "D" },
            { CDRChord.Dm, "Dm" },
            { CDRChord.Dsos, "D#" },
            { CDRChord.Dsosm, "D#m" },
            { CDRChord.E, "E" },
            { CDRChord.Em, "Em" },
            { CDRChord.F, "F" },
            { CDRChord.Fm, "Fm" },
            { CDRChord.Fsos, "F#" },
            { CDRChord.Fsosm, "F#m" },
            { CDRChord.G, "G" },
            { CDRChord.Gm, "Gm" },
            { CDRChord.Gsos, "G#" },
            { CDRChord.Gsosm, "G#m" },
            { CDRChord.A, "A" },
            { CDRChord.Am, "Am" },
            { CDRChord.Asos, "A#" },
            { CDRChord.Asosm, "A#m" },
            { CDRChord.B, "B" },
            { CDRChord.Bm, "Bm" }
        };
    }
}