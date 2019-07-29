using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Database;
using System;

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

        private readonly BufferedContainer screen;

        public Toolbar(BufferedContainer Screen)
        {
            RelativeSizeAxes = Axes.X;
            Size = new Vector2(1, HEIGHT);

            screen = Screen;
        }

        [BackgroundDependencyLoader(true)]
        private void load(RhythmicGame game)
        {
            Children = new Drawable[]
            {
                new ToolbarBackground(screen),
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
                        new ToolbarNotificationButton(),
                    }
                }
            };

            State.ValueChanged += visibility =>
            {
                if (overlayActivationMode.Value == OverlayActivation.Disabled)
                    State.Value = Visibility.Hidden;
            };

            if (game != null)
                overlayActivationMode.BindTo(game.OverlayActivationMode);
        }

        public class ToolbarBackground : Container
        {
            private readonly Box solidBackground;
            private readonly Box gradientBackground;

            public ToolbarBackground(BufferedContainer screen)
            {
                RelativeSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    new BufferedContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        BackgroundColour = Color4.Black,
                        BlurSigma = new Vector2(15),
                        Child = screen.CreateView().With(d =>
                        {
                            d.RelativeSizeAxes = Axes.Both;
                            d.SynchronisedDrawQuad = true;
                        })
                    },
                    solidBackground = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black.Opacity(0.2f),
                        Alpha = alpha_normal,
                    },
                    gradientBackground = new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Anchor = Anchor.BottomLeft,
                        Alpha = 0,
                        Height = 90,
                        Colour = ColourInfo.GradientVertical(
                            Color4.Black.Opacity(0.5f), Color4.Black.Opacity(0)),
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(RhythmicStore store)
            {
                solidBackground.Colour = ColourInfo.GradientHorizontal(
                    store.SecondaryColour?.Value.Opacity(0.3f) ?? Color4.Black.Opacity(0.3f),
                    store.SecondaryColour?.Value.Opacity(0.2f) ?? Color4.Black.Opacity(0.2f)
                    );
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
