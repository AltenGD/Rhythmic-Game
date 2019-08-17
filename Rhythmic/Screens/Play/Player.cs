using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.MathUtils;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System;

namespace Rhythmic.Screens.Play
{
    public class Player : Container, IRequireHighFrequencyMousePosition
    {
        public playerCircle PlayerCircle;

        private healthIndicatorCircle healthIndicator;

        private StopwatchClock clock;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        private PlayerMeta playerMetadata => collection?.CurrentBeatmap?.Value?.Player;

        public BindableFloat Health = new BindableFloat();

        public Action OnDeath;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(PlayerCircle = new playerCircle
            {
                Origin = Anchor.Centre
            });

            Add(healthIndicator = new healthIndicatorCircle(Health)
            {
                Origin = Anchor.Centre
            });

            clock = new StopwatchClock(true);
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        public override bool PropagatePositionalInputSubTree => IsPresent;

        private Vector2? lastPosition;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (lastPosition.HasValue && Precision.AlmostEquals(e.ScreenSpaceMousePosition, lastPosition.Value))
                return false;

            lastPosition = e.ScreenSpaceMousePosition;

            PlayerCircle.MoveTo(e.MousePosition, playerMetadata?.FollowLength ?? 300, Easing.OutExpo);
            healthIndicator.Position = e.MousePosition;
            return base.OnMouseMove(e);
        }

        public void TakeDamage()
        {
            if ((clock.ElapsedMilliseconds * Clock.Rate) > TimeSpan.FromSeconds(collection.CurrentBeatmap.Value.Player?.Cooldown ?? 5).TotalMilliseconds)
            {
                Health.Value -= 1;

                if (Health.Value == 0)
                    OnDeath?.Invoke();

                clock.Restart();

                AddExplosion(PlayerCircle.Position);
            }
        }

        public class playerCircle : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            private PlayerMeta playerMetadata => collection?.CurrentBeatmap?.Value?.Player;

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    new Circle
                    {
                        Size = new Vector2(playerMetadata?.Size ?? 30),
                        Colour = new Color4(playerMetadata?.Colour[0] ?? 255, playerMetadata?.Colour[1] ?? 255, playerMetadata?.Colour[2] ?? 255, 255)
                    }
                };
            }
        }

        private class healthIndicatorCircle : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            private PlayerMeta playerMetadata => collection?.CurrentBeatmap?.Value?.Player;

            private readonly BindableFloat health = new BindableFloat();

            private CircularProgress circle;
            private CircularProgress circleBackdrop;

            public healthIndicatorCircle(BindableFloat Health)
            {
                health.BindTo(Health);
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    circle = new CircularProgress
                    {
                        Size = new Vector2(playerMetadata?.Size * 2.1f ?? 30 * 2.1f),
                        InnerRadius = 0.2f,
                        Alpha = 0.5f
                    },
                    circleBackdrop = new CircularProgress
                    {
                        Size = new Vector2(playerMetadata?.Size * 2.1f ?? 30 * 2.1f),
                        InnerRadius = 0.2f,
                        Alpha = 0.25f
                    },
                };

                health.Value = playerMetadata.Health;

                health.ValueChanged += value =>
                {
                    circle.FillTo(value.NewValue / playerMetadata.Health, 1000, Easing.OutExpo);
                };

                circle.Current.Value = 1;
                circleBackdrop.Current.Value = 1;
            }
        }

        private void AddExplosion(Vector2 Position)
        {
            CircularProgress explosionCirc;
            Add(explosionCirc = new CircularProgress
            {
                Size = new Vector2(0),
                Position = Position,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Current = new Bindable<double>(1),
                Alpha = 0.5f,
            });

            explosionCirc.TransformTo(nameof(explosionCirc.InnerRadius), 0f, 1000, Easing.OutExpo);
            explosionCirc.ResizeTo(new Vector2(50f * (float)(collection?.CurrentBeatmap?.Value?.Player?.Size / 10)), 1000, Easing.OutExpo);
        }
    }
}