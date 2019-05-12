using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Colors;
using Rhythmic.Screens.MainMenu.Components;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Containers;
using Rhythmic.Screens.Select;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Other;
using osuTK;
using Rhythmic.Screens.Edit;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Input;
using osu.Framework.MathUtils;
using osu.Framework.Bindables;
using System;
using osu.Framework.Timing;

namespace Rhythmic.Screens.Play
{
    public class Player : Container, IRequireHighFrequencyMousePosition
    {
        public playerCircle player;

        private healthIndicatorCircle healthIndicator;

        private StopwatchClock clock;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public BindableFloat Health = new BindableFloat();

        public Action OnDeath;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(player = new playerCircle
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

            player.MoveTo(e.MousePosition, 300, Easing.OutExpo);
            healthIndicator.MoveTo(e.MousePosition, 0);
            return base.OnMouseMove(e);
        }

        public void TakeDamage()
        {
            if (clock.ElapsedMilliseconds > TimeSpan.FromSeconds(collection.CurrentBeatmap.Value.Player?.Cooldown ?? 5).TotalMilliseconds)
            {
                Health.Value -= 1;

                if (Health.Value == 0)
                    OnDeath?.Invoke();

                clock.Restart();
            }
        }

        public class playerCircle : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    new Circle
                    {
                        Size = new Vector2(collection?.CurrentBeatmap?.Value?.Player?.Size ?? 30),
                        Colour = new Color4(collection?.CurrentBeatmap?.Value?.Player?.Colour[0] ?? 255, collection?.CurrentBeatmap?.Value?.Player?.Colour[1] ?? 255, collection?.CurrentBeatmap?.Value?.Player?.Colour[2] ?? 255, 255)
                    }
                };
            }
        }

        private class healthIndicatorCircle : Container
        {
            [Resolved]
            private BeatmapCollection collection { get; set; }

            private BindableFloat health = new BindableFloat();

            private CircularProgress circle;

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
                        Size = new Vector2(collection?.CurrentBeatmap?.Value?.Player?.Size * 2.1f ?? 30 * 2.1f),
                        InnerRadius = 0.2f,
                        Alpha = 0.5f
                    }
                };

                health.Value = collection.CurrentBeatmap.Value.Player.Health;

                health.ValueChanged += value =>
                {
                    circle.Current.Value = value.NewValue / collection.CurrentBeatmap.Value.Player.Health;
                };

                circle.Current.Value = 1;
            }
        }
    }
}