using Rhythmic.Beatmap.Properties;
using Rhythmic.Screens.Backgrounds;

namespace Rhythmic.Screens.Edit
{
    public class Editor : RhythmicScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public override bool HideOverlaysOnEnter => true;

        public Editor()
        {

        }
    }
}
