using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK.Graphics;
using Rhythmic.Screens;
using Rhythmic.Screens.Backgrounds;

namespace Rhythmic.Test
{
    public class RhythmicTestScene : TestScene
    {
        public BackgroundScreenStack ScreenStack;

        public BackgroundScreenDefault DefaultBackgroundScreen;

        public RhythmicTestScene()
        {
            Add(ScreenStack = new BackgroundScreenStack
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });

            ScreenStack.Push(DefaultBackgroundScreen = new BackgroundScreenDefault());
        }
    }
}
