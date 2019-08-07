using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
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

        public override float BackgroundParallaxAmount => 0.25f;

        public readonly SongProgress Progress;

        private FailOverlay failOverlay;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected GameplayClockContainer GameplayClockContainer { get; private set; }

        protected override void LoadComplete()
        {
            collection.CurrentBeatmap.Value.Song.RestartPoint = 0;

            PlayableContainer container;

            InternalChild = GameplayClockContainer = new GameplayClockContainer(collection.CurrentBeatmap.Value, 0);

            GameplayClockContainer.UserPlaybackRate = new Bindable<double>(1);

            GameplayClockContainer.Children = new Drawable[]
            {
                container = new PlayableContainer
                {
                    Clock = GameplayClockContainer.GameplayClock,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                failOverlay = new FailOverlay
                {
                    Depth = -100,
                    RelativeSizeAxes = Axes.Both,
                    OnQuit = () => { this.Exit(); container.Expire(); collection.CurrentBeatmap.Value.Song.Restart(); },
                    //Currently doesnt work
                    OnRetry = () =>
                    {
                        GameplayClockContainer.Start();
                        GameplayClockContainer.Restart();
                        container.Restart();

                        container.player.Health.Value = collection.CurrentBeatmap.Value.Player.Health;
                        collection.CurrentBeatmap.Value.Song.Reset();

                        container.player.OnDeath = () =>
                        {
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