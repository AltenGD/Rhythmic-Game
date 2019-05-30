using osu.Framework.Graphics.Sprites;

namespace Rhythmic.Graphics.Sprites
{
    public static class RhythmicFont
    {
        /// <summary>The default font size.</summary>
        public const float DEFAULT_FONT_SIZE = 40;

        /// <summary>The default font.</summary>
        public static FontUsage Default => GetFont();

        public static FontUsage Numeric => GetFont(Typeface.Digitall);

        public static FontUsage GetFont(Typeface typeface = Typeface.Purista, float size = DEFAULT_FONT_SIZE, FontWeight weight = FontWeight.Regular, bool italics = false, bool fixedWidth = false)
            => new FontUsage(GetFamilyString(typeface, weight), size, null, italics, fixedWidth);

        private static string GetFamilyString(Typeface typeface, FontWeight weight)
        {
            switch (typeface)
            {
                case Typeface.Purista:
                    if (weight == FontWeight.Bold)
                        return "Purista-Bold";
                    else
                        return "Purista";
                case Typeface.Digitall:
                    return "Digitall";
            }

            return null;
        }

    }
    public enum Typeface
    {
        Purista,
        Digitall
    }
    public enum FontWeight
    {
        Regular,
        Medium,
        Bold
    }
}
