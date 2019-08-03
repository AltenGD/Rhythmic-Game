using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Other;
using Rhythmic.Screens.MainMenu;
using System.IO;
using System.Text;
using static System.Environment;

namespace Rhythmic.Screens
{
    public class Loader : RhythmicScreen
    {
        [Resolved]
        private BeatmapAPI API { get; set; }

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public Loader()
        {
            ValidForResume = false;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            LoadAllBeatmaps();
        }

        protected override void LogoArriving(RhythmicLogo logo, bool resuming)
        {
            base.LogoArriving(logo, resuming);

            logo.RelativePositionAxes = Axes.None;
            logo.Origin = Anchor.BottomRight;
            logo.Anchor = Anchor.BottomRight;
            logo.Position = new Vector2(-40);
            logo.Scale = new Vector2(0.2f);

            logo.Delay(500).FadeInFromZero(1000, Easing.OutQuint);
        }

        protected override void LogoSuspending(RhythmicLogo logo)
        {
            base.LogoSuspending(logo);
            logo.FadeOut(logo.Alpha * 400);
        }

        private RhythmicScreen loadableScreen;

        protected virtual RhythmicScreen CreateLoadableScreen() => new Intro();

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            LoadComponentAsync(loadableScreen = CreateLoadableScreen());

            checkIfLoaded();
        }

        private void checkIfLoaded()
        {
            if (loadableScreen.LoadState != LoadState.Ready)
            {
                Schedule(checkIfLoaded);
                return;
            }

            this.Push(loadableScreen);
        }

        private void LoadAllBeatmaps()
        {
            string path = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), "Rhythmic", "Database", "Beatmaps");

            foreach (string file in Directory.EnumerateDirectories(path))
            {
                API.GetBeatmapFromZip(file);
                BeatmapMeta level = API.ParseBeatmap(File.ReadAllText(Path.Combine(file, "level.json")));

                BeatmapMeta beatmap = new BeatmapMeta
                {
                    Level = level.Level,
                    Metadata = level.Metadata,
                    Player = level.Player,
                    SongUrl = level.SongUrl
                };

                if (beatmap.SongUrl?.StartsWith(Path.DirectorySeparatorChar) == false)
                    beatmap.SongUrl = Path.DirectorySeparatorChar + beatmap.SongUrl;

                if (beatmap.Metadata?.LogoURL?.StartsWith(Path.DirectorySeparatorChar) == false)
                    beatmap.Metadata.LogoURL = Path.DirectorySeparatorChar + beatmap.Metadata.LogoURL;

                if (!beatmap.Metadata?.BackgroundURL?.StartsWith(Path.DirectorySeparatorChar) == false)
                    beatmap.Metadata.BackgroundURL = Path.DirectorySeparatorChar + beatmap.Metadata.BackgroundURL;

                FileStream SongStream = File.OpenRead(file + beatmap.SongUrl);

                beatmap.Song = new TrackBass(SongStream);
                if (beatmap.Metadata?.LogoURL != null)
                    beatmap.Logo = Texture.FromStream(File.OpenRead(file + beatmap.Metadata.LogoURL));
                if (beatmap.Metadata?.BackgroundURL != null)
                    beatmap.Background = Texture.FromStream(File.OpenRead(file + beatmap.Metadata.BackgroundURL));

                collection.Beatmaps.Add(beatmap);
            }

            for (int i = 0; i < collection.Beatmaps.Count; i++)
            {
                collection.Beatmaps[i].ID = i;
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
