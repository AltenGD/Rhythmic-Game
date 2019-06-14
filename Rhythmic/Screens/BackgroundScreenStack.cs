using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osuTK;

namespace Rhythmic.Screens
{
    public class BackgroundScreenStack : ScreenStack
    {
        public BackgroundScreenStack(IScreen main = null)
            : base(main)
        {
            Scale = new Vector2(1.06f);
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public BackgroundScreenStack()
            : base()
        {
            Scale = new Vector2(1.06f);
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void Push(BackgroundScreen screen)
        {
            if (screen == null)
                return;

            if (EqualityComparer<BackgroundScreen>.Default.Equals((BackgroundScreen)CurrentScreen, screen))
                return;

            base.Push(screen);
        }
    }
}
