using osu.Framework.Screens;
using Rhythmic.Screens.Backgrounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhythmic.Screens.Edit
{
    public class Editor : ScreenWhiteBox
    {
        public Editor(BackgroundScreenDefault background)
        {
            background?.Next();
        }
    }
}
