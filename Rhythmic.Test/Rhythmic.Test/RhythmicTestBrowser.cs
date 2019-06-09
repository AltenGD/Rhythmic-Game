using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK.Graphics;
using Rhythmic.Screens;
using Rhythmic.Screens.Backgrounds;

namespace Rhythmic.Test
{
    public class RhythmicTestBrowser : RhythmicGameBase
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            BackgroundScreenStack screenStack;

            Add(screenStack = new BackgroundScreenStack()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Gray
            });

            screenStack.Push(new BackgroundScreenDefault());

            Add(new TestBrowser("Rhythmic"));
        }
    }
}