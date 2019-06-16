using osuTK.Graphics;
using System;

namespace Rhythmic.Extensions.Color4Extensions
{
    /// <summary>An even more extended version of Color4 from the osu!Framework.</summary>
    public static class Color4Extensions
    {
        public const double GAMMA = 2.4;

        public static double ToLinear(double color) => color <= 0.04045 ? color / 12.92 : Math.Pow((color + 0.055) / 1.055, GAMMA);

        public static double ToSRGB(double color) => color < 0.0031308 ? 12.92 * color : 1.055 * Math.Pow(color, 1.0 / GAMMA) - 0.055;

        public static Color4 Opacity(this Color4 color, float a) => new Color4(color.R, color.G, color.B, a);

        public static Color4 Opacity(this Color4 color, byte a) => new Color4(color.R, color.G, color.B, a / 255f);

        public static Color4 ToLinear(this Color4 colour) =>
            new Color4(
                (float)ToLinear(colour.R),
                (float)ToLinear(colour.G),
                (float)ToLinear(colour.B),
                colour.A);

        public static Color4 ToSRGB(this Color4 colour) =>
            new Color4(
                (float)ToSRGB(colour.R),
                (float)ToSRGB(colour.G),
                (float)ToSRGB(colour.B),
                colour.A);

        public static Color4 MultiplySRGB(Color4 first, Color4 second)
        {
            if (first.Equals(Color4.White))
                return second;

            if (second.Equals(Color4.White))
                return first;

            first = first.ToLinear();
            second = second.ToLinear();

            return new Color4(
                first.R * second.R,
                first.G * second.G,
                first.B * second.B,
                first.A * second.A).ToSRGB();
        }

        public static Color4 Multiply(Color4 first, Color4 second)
        {
            if (first.Equals(Color4.White))
                return second;

            if (second.Equals(Color4.White))
                return first;

            return new Color4(
                first.R * second.R,
                first.G * second.G,
                first.B * second.B,
                first.A * second.A);
        }

        /// <summary>Converts the colour into HSL.</summary>
        /// <param name="color">Original colour.</param>
        /// <param name="hue">Outputted hue.</param>
        /// <param name="saturation">Outputted saturation.</param>
        /// <param name="lightness">Outputted lightness.</param>
        public static void ColorToHSL(this Color4 color, out double hue, out double saturation, out double lightness)
        {
            // Get the maximum and minimum RGB components.
            double max = color.R;
            if (max < color.G) max = color.G;
            if (max < color.B) max = color.B;

            double min = color.R;
            if (min > color.G) min = color.G;
            if (min > color.B) min = color.B;

            double diff = max - min;
            lightness = (max + min) / 2;

            if (Math.Abs(diff) < 0.00001)
            {
                saturation = 0;
                hue = 0;  // H is really undefined.
            }
            else
            {
                if (lightness <= 0.5) saturation = diff / (max + min);
                else saturation = diff / (2 - max - min);

                double rDist = (max - color.R) / diff;
                double gDist = (max - color.G) / diff;
                double bDist = (max - color.B) / diff;

                if (color.R == max) hue = bDist - gDist;
                else if (color.G == max) hue = 2 + rDist - bDist;
                else hue = 4 + gDist - rDist;

                hue = hue * 60;
                if (hue < 0) hue += 360;
            }
        }

        /// <summary>Returns a lightened version of the colour.</summary>
        /// <param name="colour">Original colour</param>
        /// <param name="amount">Decimal light addition</param>
        public static Color4 Lighten(this Color4 colour, float amount) => Multiply(colour, 1 + amount);

        /// <summary>Returns a darkened version of the colour.</summary>
        /// <param name="colour">Original colour</param>
        /// <param name="amount">Percentage light reduction</param>
        public static Color4 Darken(this Color4 colour, float amount) => Multiply(colour, 1 / (1 + amount));

        /// <summary>Multiply the RGB coordinates by a scalar.</summary>
        /// <param name="colour">Original colour</param>
        /// <param name="scalar">A scalar to multiply with</param>
        /// <returns></returns>
        public static Color4 Multiply(this Color4 colour, float scalar)
        {
            if (scalar < 0)
                throw new ArgumentOutOfRangeException(nameof(scalar), scalar, "Can not multiply colours by negative values.");

            return new Color4(
                Math.Min(1, colour.R * scalar),
                Math.Min(1, colour.G * scalar),
                Math.Min(1, colour.B * scalar),
                colour.A);
        }
    }
}
