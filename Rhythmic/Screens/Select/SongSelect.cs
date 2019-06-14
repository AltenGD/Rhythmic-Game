using osu.Framework.Screens;
using Rhythmic.Screens.Backgrounds;
using osu.Framework.Graphics;
using Rhythmic.Screens.Select.Components;
using osu.Framework.Allocation;
using Rhythmic.Beatmap;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : RhythmicScreen
    {
        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public override bool DisableBeatmapOnEnter => false;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public SongSelect()
        {
            AddRangeInternal(new Drawable[]
            {
                new BeatmapLevelListing
                {
                    RelativeSizeAxes = Axes.Both,
                    selectedBeatmap = delegate
                    {
                        this.Push(new InfoScreen());
                    }
                },
            });
        }

        public override void OnResuming(IScreen last)
        {
            collection.CurrentBeatmap.Disabled = false;
            base.OnResuming(last);
        }
    }
}
