using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Drawables;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Overlays;
using System;

namespace Rhythmic.Screens.Play
{
    public class Play : RhythmicScreen
    {
        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.Disabled;

        public override bool HideOverlaysOnEnter => true;

        protected override bool AllowBackButton => false;

        public override float BackgroundParallaxAmount => 0.25f;

        /// <summary>Whether gameplay should pause when the game window focus is lost.</summary>
        protected virtual bool PauseOnFocusLost => true;

        public readonly SongProgress Progress;

        private FailOverlay failOverlay;
        private PauseOverlay pauseOverlay;

        private PlayableContainer container;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected GameplayClockContainer GameplayClockContainer { get; private set; }

        private readonly bool allowPause;
        private readonly bool showResults;

        /// <summary>Create a new player instance.</summary>
        /// <param name="allowPause">Whether pausing should be allowed. If not allowed, attempting to pause will quit.</param>
        /// <param name="showResults">Whether results screen should be pushed on completion.</param>
        public Play(bool allowPause = true, bool showResults = true)
        {
            this.allowPause = allowPause;
            this.showResults = showResults;
        }

        protected override void LoadComplete()
        {
            collection.CurrentBeatmap.Value.Song.RestartPoint = 0;

            InternalChild = GameplayClockContainer = new GameplayClockContainer(collection.CurrentBeatmap.Value, 0);

            GameplayClockContainer.UserPlaybackRate = new Bindable<double>(1);

            var clock = new DecoupleableInterpolatingFramedClock();
            clock.ChangeSource(GameplayClockContainer.sourceClock);

            GameplayClockContainer.Children = new Drawable[]
            {
                container = new PlayableContainer(clock)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                failOverlay = new FailOverlay
                {
                    Depth = -100,
                    RelativeSizeAxes = Axes.Both,
                    OnQuit = performUserRequestedExit,
                    //Currently doesnt work
                    OnRetry = Restart
                },
                pauseOverlay = new PauseOverlay
                {
                    OnResume = Resume,
                    OnRetry = Restart,
                    OnQuit = performUserRequestedExit,
                },
                new SongProgress(clock)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Objects = collection.CurrentBeatmap.Value.Level.Level,
                }
            };

            container.OnLoadComplete += delegate
            {
                collection.CurrentBeatmap.Value.Song.Restart();
                GameplayClockContainer.Start();
                container.player.OnDeath = () =>
                {
                    GameplayClockContainer.Stop();
                    failOverlay.ToggleVisibility();
                };
            };

            base.LoadComplete();
        }

        private void Restart()
        {
            GameplayClockContainer.Restart();
            container.Restart();

            container.player.Health.Value = collection.CurrentBeatmap.Value.Player.Health;
            collection.CurrentBeatmap.Value.Song.Reset();
            collection.CurrentBeatmap.Value.Song.Start();

            container.player.OnDeath = () =>
            {
                GameplayClockContainer.Stop();
                failOverlay.ToggleVisibility();
            };

            failOverlay.Retries++;
        }

        public void Resume()
        {
            pauseOverlay.Hide();

            GameplayClockContainer.Start();
        }

        private void performUserRequestedExit()
        {
            if (!this.IsCurrentScreen()) return;

            this.Exit();
            container.Expire();
            collection.CurrentBeatmap.Value.Song.Restart();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                pauseOverlay.Show();
                GameplayClockContainer.Stop();
            }

            return base.OnKeyDown(e);
        }

        private class PlayableContainer : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            public Player player;

            private DecoupleableInterpolatingFramedClock clock;

            public PlayableContainer(DecoupleableInterpolatingFramedClock clock)
            {
                this.clock = clock;
            }

            protected override void LoadComplete()
            {
                AddInternal(player = new Player
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Clock = clock
                });

                foreach (Beatmap.Properties.Level.Object.Object obj in collection.CurrentBeatmap.Value.Level.Level)
                {
                    AddInternal(new DrawableBeatmapObject(obj, player) { Depth = obj.Depth, Clock = clock });
                }

                base.LoadComplete();
            }

            public void Restart()
            {
                ClearInternal();

                AddInternal(player = new Player
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                });

                foreach (Beatmap.Properties.Level.Object.Object obj in collection.CurrentBeatmap.Value.Level.Level)
                {
                    AddInternal(new DrawableBeatmapObject(obj, player) { Depth = obj.Depth });
                }
            }
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            Alpha = 0;
            this
                .ScaleTo(0.975f)
                .ScaleTo(1, 750, Easing.OutQuint)
                .FadeIn(750);
        }

        public override bool OnExiting(IScreen next)
        {
            GameplayClockContainer.ResetLocalAdjustments();
            return base.OnExiting(next);
        }
    }
}