using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Level.Object;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Tools;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Collections.Generic;
using System.IO;
using static System.Environment;

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

            string directory = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), "Rhythmic", "Database", "Beatmaps");
            IArchive archive = ArchiveFactory.Open(path);
            foreach (IArchiveEntry entry in archive.Entries)
            {
                if (!File.Exists(Path.Combine(directory, entry.Key)))
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
            BeatmapMeta Beatmap = new BeatmapMeta
            {
                Level = new LevelMeta
                {
                    Level = new List<Object>(),
                },
                Metadata = new BeatmapMetadata
                {
                    Level = new LevelMetadata(),
                    Song = new SongMetadata()
                },
                Player = new PlayerMeta()
            };

            Beatmap.Metadata = meta.Metadata;
            Beatmap.Player = meta.Player;
            Beatmap.SongUrl = meta.SongUrl;

            return Beatmap;
        }

        public void AddObject(Object obj, BeatmapMeta meta)
            => meta.Level.Level.Add(obj);
    }
}
