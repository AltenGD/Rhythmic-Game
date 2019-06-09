using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapLogoSprite : Sprite
    {
        private BeatmapMeta beatmap;

        public BeatmapLogoSprite(BeatmapMeta beatmap)
            => this.beatmap = beatmap ?? throw new ArgumentNullException(nameof(beatmap));

        [BackgroundDependencyLoader]
        private void load()
        {
            if (beatmap.Logo != null)
                Texture = beatmap.Logo;
        }
    }
}
