using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Level.Object;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Beatmap.Properties.Metadata.User;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using osu.Framework;
using Rhythmic.Tools;
using static System.Environment;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace Rhythmic.Beatmap
{
    public class BeatmapAPI : Component
    {
        [Resolved]
        private Game game { get; set; }

        public void GetBeatmapFromZip(string path)
        {
            if (!ZipUtils.IsZipArchive(path))
                return;

            var directory = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\Beatmaps\";
            var archive = ArchiveFactory.Open(path);
            foreach (var entry in archive.Entries)
            {
                if (!File.Exists(directory + entry.Key))
                    entry.WriteToDirectory(directory, new ExtractionOptions
                    {
                        ExtractFullPath = true
                    });
            }

            archive.Dispose();
            File.Delete(path);
        }

        // Beatmap
        public BeatmapMeta ParseBeatmap(string json)
            => JsonConvert.DeserializeObject<BeatmapMeta>(json);

        public string ParseBeatmap(BeatmapMeta meta)
            => JsonConvert.SerializeObject(meta, Formatting.Indented);

        public Stream GetTrack(BeatmapMeta meta)
            => game.Resources.GetStream("Tracks/" + meta.Metadata.Level.LevelName + "/" + meta.SongUrl);

        // Beatmap editing
        public BeatmapMeta CreateNewLevel(BeatmapMeta meta)
        {
            var Beatmap = new BeatmapMeta();

            Beatmap.Level = new LevelMeta();
            Beatmap.Level.Level = new List<Object>();
            Beatmap.Level.Prefabs = new List<Prefab>();
            Beatmap.Metadata = new BeatmapMetadata();
            Beatmap.Metadata.Level = new LevelMetadata();
            Beatmap.Metadata.Level.Creator = new UserMetadata();
            Beatmap.Metadata.Song = new SongMetadata();
            Beatmap.Player = new PlayerMeta ();

            Beatmap.Metadata = meta.Metadata;
            Beatmap.Player = meta.Player;
            Beatmap.SongUrl = meta.SongUrl;

            return Beatmap;
        }

        public void AddObject(Object obj, BeatmapMeta meta)
            => meta.Level.Level.Add(obj);

        public void AddPrefab(Prefab pre, BeatmapMeta meta)
            => meta.Level.Prefabs.Add(pre);
    }
}
