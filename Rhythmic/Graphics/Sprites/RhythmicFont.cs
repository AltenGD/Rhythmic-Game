using System;
using osu.Framework.Graphics.Sprites;

namespace Rhythmic.Graphics.Sprites
{
    public static class RhythmicFont
    {
        /// <summary>The default font size.</summary>
        public const float DEFAULT_FONT_SIZE = 30;

        public static FontUsage GetFont(Typeface typeface = Typeface.Purista, float size = DEFAULT_FONT_SIZE, FontWeight weight = FontWeight.Regular, bool italics = false, bool fixedWidth = false)
            => new FontUsage(GetFamilyString(typeface, weight), size, null, italics, fixedWidth);

        private static string GetFamilyString(Typeface typeface, FontWeight weight)
        {
            switch (typeface)
            {
                case Typeface.Purista:
                    return "purista";
                case Typeface.Neogrey:
                    if (weight == FontWeight.Medium)
                        return "Neogrey Medium";
                    else
                        return "Neogrey";
                case Typeface.Audiowide:
                    return "Audiowide";
            }

            return null;
        }

        public enum Typeface
        {
            Purista,
            Neogrey,
            Audiowide,
        }

        public enum FontWeight
        {
            Regular,
            Medium
        }
    }
}
