using osu.Framework.Screens;
using Rhythmic.Screens.Backgrounds;
using osu.Framework.Graphics;
using Rhythmic.Screens.Select.Components;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : RhythmicScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public SongSelect()
        {
            AddRangeInternal(new Drawable[]
            {
                new BeatmapLevelListing
                {
                    RelativeSizeAxes = Axes.Both,
                    selectedBeatmap = delegate
                    {
                        this.Push(new Play.Play());
                    }
                },
            });
        }
    }
}
