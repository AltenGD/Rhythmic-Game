using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Screens.MainMenu.Components;
using System.IO;
using System.Text;
using static System.Environment;
using osu.Framework.Graphics.Containers;
using Rhythmic.Screens.Select;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Other;
using osuTK;
using Rhythmic.Screens.Edit;

namespace Rhythmic.Screens.MainMenu
{
    public class MainMenu : RhythmicScreen
    {
        private BackgroundScreenDefault background;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        [Resolved]
        private BeatmapAPI API { get; set; }

        private RhythmicLogo logo;

        protected override BackgroundScreen CreateBackground() => background = new BackgroundScreenDefault();

        public MainMenu()
        {
            AddRangeInternal(new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Margin = new MarginPadding(50),
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Children = new Drawable[]
                            {
                                logo = new RhythmicLogo
                                {
                                    Scale = new Vector2(0.5f),
                                    Alpha = 0
                                },
                            }
                        },
                        new ButtonSystem
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Margin = new MarginPadding(50),
                            OnPlay = () => this.Push(new SongSelect()),
                            OnEditor = () => this.Push(new Editor(background))
                        }
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadAsyncComplete();
            LoadAllBeatmaps();
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            (Background as BackgroundScreenDefault)?.Next();
            logo.MoveToX(25).MoveToX(0, 1000, Easing.OutExpo);
            logo.FadeIn(1000, Easing.OutExpo);
        }

        public override void OnEntering(IScreen last)
        {
            logo.MoveToX(25).MoveToX(0, 1000, Easing.OutExpo);
            logo.FadeIn(1000, Easing.OutExpo);
            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            logo.MoveToX(25, 1000, Easing.InExpo);
            logo.FadeOut(1000, Easing.OutExpo);
            return base.OnExiting(next);
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
