using Newtonsoft.Json;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Metadata;
using System;

//TODO: Implement storyboards
namespace Rhythmic.Beatmap.Properties
{
    /// <summary>Contains information like: BG, <see cref="LevelMeta"/>, etc</summary>
    public class BeatmapMeta
    {
        public BeatmapMetadata Metadata { get; set; }

        public LevelMeta Level { get; set; }

        public PlayerMeta Player { get; set; }

        public string SongUrl { get; set; }

        public DateTime DownloadedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public Texture Logo { get; set; }

        [JsonIgnore]
        public Texture Background { get; set; }

        [JsonIgnore]
        public Track Song { get; set; }

        [JsonIgnore]
        public int ID { get; set; }

        public override string ToString() => $"{Metadata?.Song?.Author} - {Metadata?.Song?.Name} ({Metadata?.Level?.CreatorName})";
    }
}
