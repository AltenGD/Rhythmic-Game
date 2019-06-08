using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using osu.Framework.Graphics.Containers;
using Rhythmic.Overlays;
using Rhythmic.Beatmap.Drawables;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Graphics.Sprites;
using System;
using osu.Framework.Audio;

namespace Rhythmic.Screens.Play
{
    public class Play : RhythmicScreen
    {
        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.Disabled;

        public override bool HideOverlaysOnEnter => true;

        public override float BackgroundParallaxAmount => 0f;

        public readonly SongProgress Progress;

        private FailOverlay failOverlay;
        private SpriteText trackTimer;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected GameplayClockContainer GameplayClockContainer { get; private set; }

        protected override void LoadComplete()
        {
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
                },
                trackTimer = new SpriteText
                {
                    Font = RhythmicFont.Default
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

            /*Scheduler.AddDelayed(() => 
            {
                if (GameplayClockContainer.UserPlaybackRate.Value == 0.5)
                {
                    var songClock = collection.CurrentBeatmap.Value.Song as IHasTempoAdjust;
                    GameplayClockContainer.UserPlaybackRate.Value = 1;
                    songClock.TempoAdjust = 1;
                }
                else
                {
                    var songClock = collection.CurrentBeatmap.Value.Song as IHasTempoAdjust;
                    GameplayClockContainer.UserPlaybackRate.Value = 0.5;
                    songClock.TempoAdjust = 0.5;
                }
            }, 1000, true);*/

            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();

            trackTimer.Text = TimeSpan.FromMilliseconds(GameplayClockContainer.GameplayClock.CurrentTime).ToString(@"mm\:ss\:fff");
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

                foreach (var obj in collection.CurrentBeatmap.Value.Level.Level)
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

                foreach (var obj in collection.CurrentBeatmap.Value.Level.Level)
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