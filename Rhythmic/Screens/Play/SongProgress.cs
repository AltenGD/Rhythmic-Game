using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Screens.Play
{
    public class SongProgress : OverlayContainer
    {
        private const int bottom_bar_height = 5;

        private static readonly Vector2 handle_size = new Vector2(10, 18);

        private const float transition_duration = 200;

        private readonly SongProgressBar bar;
        private readonly SongProgressInfo info;

        public Action<double> RequestSeek;

        public override bool HandleNonPositionalInput => AllowSeeking;
        public override bool HandlePositionalInput => AllowSeeking;

        private double lastHitTime => objects.LastOrDefault().Time + objects.LastOrDefault().TotalTime + 1;

        private double firstHitTime => objects.FirstOrDefault().Time;

        private IEnumerable<Object> objects;

        public IEnumerable<Object> Objects
        {
            set
            {
                objects = value;

                info.StartTime = firstHitTime;
                info.EndTime = lastHitTime;

                bar.StartTime = firstHitTime;
                bar.EndTime = lastHitTime;
            }
        }

        private readonly BindableBool replayLoaded = new BindableBool();

        public IClock ReferenceClock;

        private IClock gameplayClock;

        [BackgroundDependencyLoader(true)]
        private void load(GameplayClock clock)
        {
            if (clock != null)
                gameplayClock = clock;
        }

        public SongProgress()
        {
            Height = bottom_bar_height + handle_size.Y;
            Y = bottom_bar_height;

            Children = new Drawable[]
            {
                info = new SongProgressInfo
                {
                    Origin = Anchor.BottomLeft,
                    Anchor = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding { Bottom = bottom_bar_height },
                },
                bar = new SongProgressBar(bottom_bar_height, handle_size)
                {
                    Alpha = 0,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    OnSeek = time => RequestSeek?.Invoke(time),
                },
            };
        }

        protected override void LoadComplete()
        {
            Hide();

            replayLoaded.ValueChanged += loaded => AllowSeeking = loaded.NewValue;
            replayLoaded.TriggerChange();
        }

        private bool allowSeeking;

        public bool AllowSeeking
        {
            get => allowSeeking;
            set
            {
                if (allowSeeking == value) return;

                allowSeeking = value;
                updateBarVisibility();
            }
        }

        private void updateBarVisibility()
        {
            bar.FadeTo(allowSeeking ? 1 : 0, transition_duration, Easing.In);
            this.MoveTo(new Vector2(0, allowSeeking ? 0 : bottom_bar_height), transition_duration, Easing.In);

            info.Margin = new MarginPadding { Bottom = Height - (allowSeeking ? 0 : handle_size.Y) };
        }

        protected override void PopIn()
        {
            updateBarVisibility();
            this.FadeIn(500, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(100);
        }

        protected override void Update()
        {
            base.Update();

            if (objects == null)
                return;

            double gameplayTime = gameplayClock?.CurrentTime ?? Time.Current;
            double frameStableTime = ReferenceClock?.CurrentTime ?? gameplayTime;

            double progress = Math.Min(1, (frameStableTime - firstHitTime) / (lastHitTime - firstHitTime));

            bar.CurrentTime = gameplayTime;
        }
    }
}
