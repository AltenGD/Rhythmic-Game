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

            Add(new TestBrowser("Rhythmic"));
        }
    }
}