using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using osu.Framework.Graphics.Containers;
using Rhythmic.Overlays;
using Rhythmic.Beatmap.Drawables;
using osu.Framework.Bindables;

namespace Rhythmic.Screens.Play
{
    public class Play : RhythmicScreen
    {
        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.Disabled;

        public override bool HideOverlaysOnEnter => true;

        public override float BackgroundParallaxAmount => 0f;

        public readonly SongProgress Progress;

        private FailOverlay failOverlay;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected GameplayClockContainer GameplayClockContainer { get; private set; }

        protected override void LoadComplete()
        {
            PlayableContainer container;

            InternalChild = GameplayClockContainer = new GameplayClockContainer(collection.CurrentBeatmap.Value, 0);

            GameplayClockContainer.UserPlaybackRate = new Bindable<double>(0.1);

            LoadComponentAsync(container = new PlayableContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            }, GameplayClockContainer.Add);

            GameplayClockContainer.Children = new Drawable[]
            {
                failOverlay = new FailOverlay
                {
                    Depth = -100,
                    RelativeSizeAxes = Axes.Both,
                    OnQuit = () => { this.Exit(); container.Expire(); collection.CurrentBeatmap.Value.Song.Restart(); },
                    OnRetry = () =>
                    {
                        GameplayClockContainer.Restart();
                        container.Restart();

                        container.player.Health.Value = collection.CurrentBeatmap.Value.Player.Health;
                        collection.CurrentBeatmap.Value.Song.Restart();
                        container.player.OnDeath = () =>
                        {
                            container.Stop();
                            GameplayClockContainer.Stop();
                            failOverlay.ToggleVisibility();
                            collection.CurrentBeatmap.Value.Song.Stop();
                        };

                        failOverlay.Retries++;
                    }
                },
                new SongProgress
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Objects = collection.CurrentBeatmap.Value.Level.Level,
                }
            };

            GameplayClockContainer.Start();

            container.OnLoadComplete += delegate
            {
                collection.CurrentBeatmap.Value.Song.Restart();
                container.player.OnDeath = () =>
                {
                    container.Stop();
                    GameplayClockContainer.Stop();
                    failOverlay.ToggleVisibility();
                    collection.CurrentBeatmap.Value.Song.Stop();
                };
            };

            base.LoadComplete();
        }

        private class PlayableContainer : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            public Player player;

            protected override void LoadComplete()
            {
                Add(player = new Player
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                });

                foreach (var obj in collection.CurrentBeatmap.Value.Level.Level)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Add(new DrawableBeatmapObject(obj, player) { Depth = obj.Depth });
                    }, obj.Time);
                }

                base.LoadComplete();
            }

            public void Stop()
            {
                Scheduler.CancelDelayedTasks();
            }

            public void Restart()
            {
                Clear();

                Add(player = new Player
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                });

                foreach (var obj in collection.CurrentBeatmap.Value.Level.Level)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Add(new DrawableBeatmapObject(obj, player));
                    }, obj.Time);
                }
            }
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            Alpha = 0;
            this
                .ScaleTo(0.7f)
                .ScaleTo(1, 750, Easing.OutQuint)
                .Delay(250)
                .FadeIn(250);
        }

        public override bool OnExiting(IScreen next)
        {
            GameplayClockContainer.ResetLocalAdjustments();
            return base.OnExiting(next);
        }
    }
}