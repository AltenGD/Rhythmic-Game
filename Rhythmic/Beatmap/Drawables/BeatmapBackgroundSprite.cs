using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapBackgroundSprite : Sprite
    {
        private BeatmapMeta beatmap;

        public BeatmapBackgroundSprite(BeatmapMeta beatmap)
            => this.beatmap = beatmap ?? throw new ArgumentNullException(nameof(beatmap));

        [BackgroundDependencyLoader]
        private void load()
        {
            if (beatmap.Background != null)
                Texture = beatmap.Background;
        }
    }
}
