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

        /// <summary>Retrieves a <see cref="FontUsage"/>.</summary>
        /// <param name="typeface">The font typeface.</param>
        /// <param name="size">The size of the text in local space. For a value of 40, a single line will have a height of 40px.</param>
        /// <param name="weight">The font weight.</param>
        /// <param name="italics">Whether the font is italic.</param>
        /// <param name="fixedWidth">Whether all characters should be spaced the same distance apart.</param>
        /// <returns>The <see cref="FontUsage"/>.</returns>
        public static FontUsage GetFont(Typeface typeface = Typeface.Purista, float size = DEFAULT_FONT_SIZE, FontWeight weight = FontWeight.Regular, bool italics = false, bool fixedWidth = false)
            => new FontUsage(GetFamilyString(typeface), size, GetWeightString(typeface, weight), italics, fixedWidth);

        /// <summary>Retrieves the string representation of a <see cref="Typeface"/>.</summary>
        /// <param name="typeface">The <see cref="Typeface"/>.</param>
        /// <returns>The string representation.</returns>
        private static string GetFamilyString(Typeface typeface)
        {
            switch (typeface)
            {
                case Typeface.Purista:
                     return "Purista";
                case Typeface.Digitall:
                    return "Digitall";
                case Typeface.Audiowide:
                    return "Audiowide";
            }

            return null;
        }

        /// <summary>Retrieves the string representation of a <see cref="FontWeight"/>.</summary>
        /// <param name="typeface">The <see cref="Typeface"/>.</param>
        /// <param name="weight">The <see cref="FontWeight"/>.</param>
        /// <returns>The string representation of <paramref name="weight"/> in the specified <paramref name="typeface"/>.</returns>
        public static string GetWeightString(Typeface typeface, FontWeight weight)
            => GetWeightString(GetFamilyString(typeface), weight);

        /// <summary>Retrieves the string representation of a <see cref="FontWeight"/>.</summary>
        /// <param name="family">The family string.</param>
        /// <param name="weight">The <see cref="FontWeight"/>.</param>
        /// <returns>The string representation of <paramref name="weight"/> in the specified <paramref name="family"/>.</returns>
        public static string GetWeightString(string family, FontWeight weight)
        {
            string weightString = weight.ToString();

            if (family != GetFamilyString(Typeface.Purista) && weight == FontWeight.Regular)
                weightString = string.Empty;

            return weightString;
        }
    }

    public enum Typeface
    {
        Purista,
        Digitall,
        Audiowide
    }
    public enum FontWeight
    {
        Bold,
        SemiBold,
        Regular,
        Light,
        Thin
    }
}
