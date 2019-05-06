using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Beatmap;
using osu.Framework;
using System;
using Rhythmic.Visualizers;
using Rhythmic.Other;

namespace Rhythmic.Screens.MainMenu
{
    public class Intro : RhythmicScreen
    {
        public bool DidLoadMenu;

        private MainMenu mainMenu;

        [Resolved]
        private AudioManager audio { get; set; }

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBlack();

        private Track track;
        private DatabasedBeatmap introBeatmap;
        private RhythmicLogo intro;

        [BackgroundDependencyLoader]
        private void load(Game game, BeatmapCollection collection)
        {
            introBeatmap = new DatabasedBeatmap();

            introBeatmap.Background = game.Resources.GetStream(@"Tracks/Intro/bg.png");
            introBeatmap.Song = new TrackBass(game.Resources.GetStream(@"Tracks/Intro/song.mp3"));

            track = introBeatmap.Song;

            collection.CurrentBeatmap.Value = introBeatmap;

            collection.CurrentBeatmap.ValueChanged += delegate
            {
                track.Stop();
                Console.WriteLine("Stop");
                audio.UnregisterItem(track);
                introBeatmap = collection.CurrentBeatmap.Value;
                Console.WriteLine("Set");
                restartTrack();
                Console.WriteLine("Restart");
            };

            collection.CurrentBeatmap.Value.Song.Completed += delegate
            {
                Console.WriteLine("Completed!");
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
                track.Seek(TimeSpan.FromSeconds(162.7).TotalMilliseconds);
                track.Start();

                LoadComponentAsync(mainMenu = new MainMenu());

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
            audio.AddItem(collection.CurrentBeatmap.Value.Song);
            Console.WriteLine("Add");
            collection.CurrentBeatmap.Value.Song.Start();
            Console.WriteLine("Start CurrentBeatmap");
        }
    }
}
