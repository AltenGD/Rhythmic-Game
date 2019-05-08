using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Overlays;
using Rhythmic.Screens.Backgrounds;
using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.UserInterface;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : ScreenWhiteBox
    {
        public SongSelect(BackgroundScreenDefault background)
        {
            background?.Next();

            AddInternal(new Button
            {
                Size = new osuTK.Vector2(120, 80),
                Action = delegate
                {
                    this.Push(new Play.Play());
                }
            });
        }
    }
}
