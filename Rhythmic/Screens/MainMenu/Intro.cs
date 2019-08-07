using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Other;
using Rhythmic.Screens.Backgrounds;
using System;

namespace Rhythmic.Screens.MainMenu
{
    public class Intro : RhythmicScreen
    {
        public bool DidLoadMenu;

        [Resolved]
        private MainMenu mainMenu { get; set; }

        [Resolved]
        private AudioManager audio { get; set; }

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBlack();

        private Track track;
        private BeatmapMeta introBeatmap;
        private RhythmicLogo intro;

        [Resolved]
        private Game game { get; set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            introBeatmap = new BeatmapMeta
            {
                Background = Texture.FromStream(game.Resources.GetStream(@"Tracks/Intro/bg.png")),
                Song = new TrackBass(game.Resources.GetStream(@"Tracks/Intro/song.mp3")),
                Metadata = new BeatmapMetadata
                {
                    Song = new SongMetadata
                    {
                        Author = "Kobaryo",
                        Name = "Villain Virus ( feat. Camellia )"
                    }
                }
            };

            track = introBeatmap.Song;

            collection.CurrentBeatmap.Value = introBeatmap;

            collection.CurrentBeatmap.ValueChanged += delegate
            {
                track.Stop();
                introBeatmap = collection.CurrentBeatmap.Value;
                restartTrack();
            };

            AddInternal(intro = new RhythmicLogo
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
            });

            intro.ScaleTo(1);
            intro.FadeIn();
            intro.PlayIntro();

            Scheduler.AddDelayed(() =>
            {
                audio.AddItem(track);
                track.Seek(206500);
                track.Start();

                Scheduler.AddDelayed(() =>
                {
                    DidLoadMenu = true;
                    this.Push(mainMenu);
                }, delay_step_one);
            }, delay_step_two);
        }

        private const double delay_step_one = 2000;
        private const double delay_step_two = 600;

        public const int EXIT_DELAY = 3000;

        public override void OnSuspending(IScreen next)
        {
            this.FadeOut(300);
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            //cancel exiting if we haven't loaded the menu yet.
            return !DidLoadMenu;
        }

        public override void OnResuming(IScreen last)
        {
            this.FadeIn(300);

            double fadeOutTime = EXIT_DELAY;
            //we also handle the exit transition.
            fadeOutTime = 500;

            Scheduler.AddDelayed(this.Exit, fadeOutTime);

            //don't want to fade out completely else we will stop running updates and shit will hit the fan.
            Game.FadeTo(0.01f, fadeOutTime);

            base.OnResuming(last);
        }

        private void restartTrack()
        {
            if (collection?.CurrentBeatmap?.Value?.Song != null)
            {
                audio.AddItem(collection.CurrentBeatmap.Value.Song);
                collection.CurrentBeatmap.Value.Song.Start();
            }
        }
    }
}
