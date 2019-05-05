using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Metadata;

//TODO: Implement storyboards
namespace Rhythmic.Beatmap.Properties
{
    public class BeatmapMeta
    {
        public BeatmapMetadata Metadata { get; set; }

        public LevelMeta Level { get; set; }

        public PlayerMeta Player { get; set; }

        public string SongUrl { get; set; }
    }
}
