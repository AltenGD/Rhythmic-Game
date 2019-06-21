using System.Collections.Generic;
using osu.Framework.Graphics.Containers;

namespace Rhythmic.Beatmap.Properties.Metadata
{
    public class BeatmapMetadata
    {
        public SongMetadata Song { get; set; }

        public LevelMetadata Level { get; set; }

        public string LogoURL { get; set; }

        public string BackgroundURL { get; set; }
    }
}