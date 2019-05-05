using osu.Framework.Testing;

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