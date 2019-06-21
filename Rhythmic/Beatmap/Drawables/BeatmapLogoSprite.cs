using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Beatmap.Properties;
using System;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapLogoSprite : Sprite
    {
        private readonly BeatmapMeta beatmap;

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
