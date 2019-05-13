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
    public class SongSelect : RhythmicScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public SongSelect()
        {
            AddInternal(new Button
            {
                Size = new osuTK.Vector2(150, 40),
                Action = delegate
                {
                    this.Push(new Play.Play());
                }
            });
        }
    }
}
