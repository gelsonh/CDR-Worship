namespace CDR_Worship.Models.Enums
{
    public enum CDRChord
    {
        C,
        D,
        E,
        F,
        G,
        A,
        B,
        Fsos,   // F#
        Csos,   // C#
        Bb,
        Cm,
        Dm,
        Em,
        Fm,
        Gm,
        Am,
        Bm,
        Fsosm,  // F#m
        Csosm,  // C#m
        Bbm,
        Gb,
        Gsos,   // G#
        Gsosm   // G#m
    }

    public static class CDRChordMapper
    {
        public static readonly Dictionary<CDRChord, string> ChordNames = new()
        {
            { CDRChord.C, "C" },
            { CDRChord.D, "D" },
            { CDRChord.E, "E" },
            { CDRChord.F, "F" },
            { CDRChord.G, "G" },
            { CDRChord.A, "A" },
            { CDRChord.B, "B" },
            { CDRChord.Fsos, "F#" },
            { CDRChord.Csos, "C#" },
            { CDRChord.Bb, "Bb" },
            { CDRChord.Cm, "Cm" },
            { CDRChord.Dm, "Dm" },
            { CDRChord.Em, "Em" },
            { CDRChord.Fm, "Fm" },
            { CDRChord.Gm, "Gm" },
            { CDRChord.Am, "Am" },
            { CDRChord.Bm, "Bm" },
            { CDRChord.Fsosm, "F#m" },
            { CDRChord.Csosm, "C#m" },
            { CDRChord.Bbm, "Bbm" },
            { CDRChord.Gb, "Gb" },
            { CDRChord.Gsos, "G#" },
            { CDRChord.Gsosm, "G#m" }
        };
    }
}