using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Overlays;
using Rhythmic.Screens.Backgrounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : ScreenWhiteBox
    {
        public SongSelect(BackgroundScreenDefault background)
        {
            background?.Next();
        }
    }
}
