using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapBackgroundSprite : Sprite
    {
        private BeatmapMeta beatmap;

        [Resolved]
        private TextureStore store { get; set; }

        public BeatmapBackgroundSprite(BeatmapMeta beatmap)
            => this.beatmap = beatmap ?? throw new ArgumentNullException(nameof(beatmap));

        [BackgroundDependencyLoader]
        private void load()
        {
            Texture = beatmap.Background ?? store.Get("Backgrounds/bg1");
        }
    }
}
