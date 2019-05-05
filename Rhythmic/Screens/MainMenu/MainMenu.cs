using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;

namespace Rhythmic.Screens.MainMenu
{
    public class MainMenu : Screen
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        [Resolved]
        private BeatmapAPI API { get; set; }

        protected override void LoadComplete()
        {
            LoadAllBeatmaps();
            base.LoadAsyncComplete();
        }

        private void LoadAllBeatmaps()
        {
            var path = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\Beatmaps\";

            foreach (var file in Directory.EnumerateDirectories(path))
            {
                API.GetBeatmapFromZip(file);
                var level = API.ParseBeatmap(File.ReadAllText(file + @"\level.json"));

                var beatmap = new DatabasedBeatmap();
                beatmap.Level = level.Level;
                beatmap.Metadata = level.Metadata;
                beatmap.Player = level.Player;
                beatmap.SongUrl = level.SongUrl;

                if (!beatmap.SongUrl.StartsWith(@"\"))
                    beatmap.SongUrl = Concat(@"\", beatmap.SongUrl);

                //if (!beatmap.Metadata.LogoURL.StartsWith(@"\"))
                //    beatmap.Metadata.LogoURL = Concat(@"\", beatmap.Metadata.LogoURL);

                //if (!beatmap.Metadata.BackgroundURL.StartsWith(@"\"))
                //    beatmap.Metadata.BackgroundURL = Concat(@"\", beatmap.Metadata.BackgroundURL);

                var SongStream = File.OpenRead(file + beatmap.SongUrl);

                beatmap.Song = new TrackBass(SongStream);
                //beatmap.Logo = File.OpenRead(file + beatmap.SongUrl);
                //beatmap.Background = File.OpenRead(file + beatmap.Metadata.BackgroundURL);

                collection.Beatmaps.Add(beatmap);
            }
        }

        private string Concat(string string1, string string2)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string1);
            sb.Append(string2);
            return sb.ToString();
        }
    }
}
