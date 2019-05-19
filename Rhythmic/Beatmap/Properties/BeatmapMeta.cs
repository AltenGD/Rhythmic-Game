using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Metadata;
using System;

//TODO: Implement storyboards
namespace Rhythmic.Beatmap.Properties
{
    public class BeatmapMeta
    {
        public BeatmapMetadata Metadata { get; set; }

        public LevelMeta Level { get; set; }

        public PlayerMeta Player { get; set; }

        public string SongUrl { get; set; }

        public DateTime DownloadedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
