using osuTK.Graphics;
using System;

namespace Rhythmic.Graphics.Colors
{
    /// <summary>Provides the colors of the application and functions related to them.</summary>
    public static class RhythmicColors
    {
        public static Color4 Gray(float amt) => new Color4(amt, amt, amt, 1f);
        public static Color4 Gray(byte amt) => new Color4(amt, amt, amt, 255);

        public static Color4 FromHex(string hex)
        {
            if (hex[0] == '#')
                hex = hex.Substring(1);

            switch (hex.Length)
            {
                default:
                    throw new ArgumentException(@"Invalid hex string length!");
                case 3:
                    return new Color4(
                        (byte)(Convert.ToByte(hex.Substring(0, 1), 16) * 17),
                        (byte)(Convert.ToByte(hex.Substring(1, 1), 16) * 17),
                        (byte)(Convert.ToByte(hex.Substring(2, 1), 16) * 17),
                        255);
                case 6:
                    return new Color4(
                        Convert.ToByte(hex.Substring(0, 2), 16),
                        Convert.ToByte(hex.Substring(2, 2), 16),
                        Convert.ToByte(hex.Substring(4, 2), 16),
                        255);
            }
        }

        public static readonly Color4 RedLighter = FromHex(@"FFCCCC");
        public static readonly Color4 RedLight = FromHex(@"FE8B8B");
        public static readonly Color4 Red = FromHex(@"EE5B5B");
        public static readonly Color4 RedDark = FromHex(@"CF4A4A");
        public static readonly Color4 RedDarker = FromHex(@"631D1D");

        public static readonly Color4 BlueLighter = FromHex(@"CCD7FF");
        public static readonly Color4 BlueLight = FromHex(@"8BA4FE");
        public static readonly Color4 Blue = FromHex(@"5B7BEE");
        public static readonly Color4 BlueDark = FromHex(@"4A67CF");
        public static readonly Color4 BlueDarker = FromHex(@"1D2C63");

        public static readonly Color4 GreenLighter = FromHex(@"9AFF8A");
        public static readonly Color4 GreenLight = FromHex(@"9BFE8B");
        public static readonly Color4 Green = FromHex(@"70EE5B");
        public static readonly Color4 GreenDark = FromHex(@"5DCF4A");
        public static readonly Color4 GreenDarker = FromHex(@"27631D");

        public static readonly Color4 OrangeLighter = FromHex(@"FFE1CC");
        public static readonly Color4 OrangeLight = FromHex(@"FEBB8B");
        public static readonly Color4 Orange = FromHex(@"EE9A5D");
        public static readonly Color4 OrangeDark = FromHex(@"CF814A");
        public static readonly Color4 OrangeDarker = FromHex(@"633A1D");

        public static readonly Color4 YellowLighter = FromHex(@"FFFFCC");
        public static readonly Color4 YellowLight = FromHex(@"FEFE8B");
        public static readonly Color4 Yellow = FromHex(@"EEEE5D");
        public static readonly Color4 YellowDark = FromHex(@"CFCF4A");
        public static readonly Color4 YellowDarker = FromHex(@"9E9E2E");

        public static readonly Color4 PurpleLighter = FromHex(@"D3CCFF");
        public static readonly Color4 PurpleLight = FromHex(@"9B8BFE");
        public static readonly Color4 Purple = FromHex(@"725DEE");
        public static readonly Color4 PurpleDark = FromHex(@"5C4ACF");
        public static readonly Color4 PurpleDarker = FromHex(@"271D63");

        public static readonly Color4 PinkLighter = FromHex(@"FECCFF");
        public static readonly Color4 PinkLight = FromHex(@"F690F9");
        public static readonly Color4 Pink = FromHex(@"EB5DEE");
        public static readonly Color4 PinkeDark = FromHex(@"CC4ACF");
        public static readonly Color4 PinkDarker = FromHex(@"611D63");

        public static readonly Color4 HotPinkLighter = FromHex(@"FFCCDE");
        public static readonly Color4 HotPinkLight = FromHex(@"FE8BB4");
        public static readonly Color4 HotPink = FromHex(@"EE5D92");
        public static readonly Color4 HotPinkDark = FromHex(@"CF4A7A");
        public static readonly Color4 HotPinkDarker = FromHex(@"631D36");

        public static readonly Color4 Gray0 = FromHex(@"000");
        public static readonly Color4 Gray1 = FromHex(@"111");
        public static readonly Color4 Gray2 = FromHex(@"222");
        public static readonly Color4 Gray3 = FromHex(@"333");
        public static readonly Color4 Gray4 = FromHex(@"444");
        public static readonly Color4 Gray5 = FromHex(@"555");
        public static readonly Color4 Gray6 = FromHex(@"666");
        public static readonly Color4 Gray7 = FromHex(@"777");
        public static readonly Color4 Gray8 = FromHex(@"888");
        public static readonly Color4 Gray9 = FromHex(@"999");
        public static readonly Color4 GrayA = FromHex(@"aaa");
        public static readonly Color4 GrayB = FromHex(@"bbb");
        public static readonly Color4 GrayC = FromHex(@"ccc");
        public static readonly Color4 GrayD = FromHex(@"ddd");
        public static readonly Color4 GrayE = FromHex(@"eee");
        public static readonly Color4 GrayF = FromHex(@"fff");
    }
}
