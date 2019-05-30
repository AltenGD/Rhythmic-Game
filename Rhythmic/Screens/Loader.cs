using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Screens;
using Rhythmic.Other;
using Rhythmic.Screens.MainMenu;
using Rhythmic.Beatmap;
using System.IO;
using static System.Environment;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using System.Text;
using Rhythmic.Beatmap.Properties;

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
            var path = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\Beatmaps\";

            foreach (var file in Directory.EnumerateDirectories(path))
            {
                API.GetBeatmapFromZip(file);
                var level = API.ParseBeatmap(File.ReadAllText(file + @"\level.json"));

                var beatmap = new BeatmapMeta();
                beatmap.Level = level.Level;
                beatmap.Metadata = level.Metadata;
                beatmap.Player = level.Player;
                beatmap.SongUrl = level.SongUrl;

                if (!beatmap.SongUrl.StartsWith(@"\"))
                    beatmap.SongUrl = Concat(@"\", beatmap.SongUrl);

                if (!beatmap.Metadata.LogoURL.StartsWith(@"\"))
                    beatmap.Metadata.LogoURL = Concat(@"\", beatmap.Metadata.LogoURL);

                if (!beatmap.Metadata.BackgroundURL.StartsWith(@"\"))
                    beatmap.Metadata.BackgroundURL = Concat(@"\", beatmap.Metadata.BackgroundURL);

                var SongStream = File.OpenRead(file + beatmap.SongUrl);

                beatmap.Song = new TrackBass(SongStream);
                beatmap.Logo = Texture.FromStream(File.OpenRead(file + beatmap.Metadata.LogoURL));
                beatmap.Background = Texture.FromStream(File.OpenRead(file + beatmap.Metadata.BackgroundURL));

                collection.Beatmaps.Add(beatmap);
            }

            for (var i = 0; i < collection.Beatmaps.Count; i++)
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
