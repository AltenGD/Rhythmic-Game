﻿using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Toolbar
{
    public class Toolbar : OverlayContainer
    {
        public const float HEIGHT = 40;
        public const float TOOLTIP_HEIGHT = 30;

        public Action OnHome;

        protected override bool BlockPositionalInput => false;

        private const double transition_time = 500;

        private const float alpha_hovering = 0.8f;
        private const float alpha_normal = 0.6f;

        private readonly Bindable<OverlayActivation> overlayActivationMode = new Bindable<OverlayActivation>(OverlayActivation.All);

        public Toolbar()
        {
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(1, HEIGHT);
        }

        [BackgroundDependencyLoader(true)]
        private void load(RhythmicGame game)
        {
            Children = new Drawable[]
            {
                new ToolbarBackground(),
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new ToolbarSettingsButton(),
                        new ToolbarHomeButton
                        {
                            Action = () => OnHome?.Invoke()
                        },
                    }
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new ToolbarMusicButton(),
                        //userButton = new ToolbarUserButton(),
                        new ToolbarNotificationButton(),
                    }
                }
            };

            StateChanged += visibility =>
            {
                if (overlayActivationMode.Value == OverlayActivation.Disabled)
                    State = Visibility.Hidden;
            };

            //if (game != null)
                //overlayActivationMode.BindTo(game.OverlayActivationMode);
        }

        public class ToolbarBackground : Container
        {
            private readonly Box solidBackground;
            private readonly Box gradientBackground;

            public ToolbarBackground()
            {
                RelativeSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    solidBackground = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = RhythmicColors.Gray(0.1f),
                        Alpha = alpha_normal,
                    },
                    gradientBackground = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Anchor = Anchor.BottomLeft,
                        Alpha = 0,
                        Height = 90,
                        Colour = ColourInfo.GradientVertical(
                            RhythmicColors.Gray(0.1f).Opacity(0.5f), RhythmicColors.Gray(0.1f).Opacity(0)),
                    },
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                solidBackground.FadeTo(alpha_hovering, transition_time, Easing.OutQuint);
                gradientBackground.FadeIn(transition_time, Easing.OutQuint);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                solidBackground.FadeTo(alpha_normal, transition_time, Easing.OutQuint);
                gradientBackground.FadeOut(transition_time, Easing.OutQuint);
            }
        }

        protected override void PopIn()
        {
            this.MoveToY(0, transition_time, Easing.OutQuint);
            this.FadeIn(transition_time / 2, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.MoveToY(-DrawSize.Y, transition_time, Easing.OutQuint);
            this.FadeOut(transition_time);
        }
    }
}
