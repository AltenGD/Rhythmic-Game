using osu.Framework.Screens;
using Rhythmic.Screens.Backgrounds;
using osu.Framework.Allocation;
using Rhythmic.Beatmap;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : RhythmicScreen
    {
        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public SongSelect()
        {

        }

        public override void OnResuming(IScreen last)
        {
            collection.CurrentBeatmap.Disabled = false;
            base.OnResuming(last);
        }
    }
}
