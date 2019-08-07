using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Containers;
using System;

namespace Rhythmic.Overlays.Notifications
{
    public abstract class Notification : Container
    {
        /// <summary>User requested close.</summary>
        public event Action Closed;

        /// <summary>Whether this notification should forcefully display itself.</summary>
        public virtual bool IsImportant => true;

        /// <summary>Run on user activating the notification. Return true to close.</summary>
        public Func<bool> Activated;

        /// <summary>Should we show at the top of our section on display?</summary>
        public virtual bool DisplayOnTop => true;

        private readonly CloseButton closeButton;
        protected Container IconContent;
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected Container NotificationContent;

        public virtual bool Read { get; set; }

        protected Notification()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            AddRangeInternal(new Drawable[]
            {
                NotificationContent = new Container
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 400,
                    AutoSizeEasing = Easing.OutQuint,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Width = 0.3f,
                                    Alpha = 0.4f,
                                    Colour = Color4.Black
                                },
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Width = 0.7f,
                                    Colour = ColourInfo.GradientHorizontal(Color4.Black, Color4.Black.Opacity(0.0f)),
                                    Alpha = 0.4f
                                },
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Padding = new MarginPadding
                            {
                                Horizontal = 20,
                                Vertical = 5
                            },
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                IconContent = new Container
                                {
                                    Size = new Vector2(40),
                                    Masking = true,
                                    CornerRadius = 5,
                                },
                                content = new Container
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Padding = new MarginPadding
                                    {
                                        Left = 45,
                                        Right = 30
                                    },
                                }
                            }
                        },
                        closeButton = new CloseButton
                        {
                            Alpha = 0,
                            Action = Close,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Margin = new MarginPadding
                            {
                                Right = 5
                            },
                        }
                    }
                }
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            closeButton.FadeIn(75);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            closeButton.FadeOut(75);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Activated?.Invoke() ?? true)
                Close();

            return true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            this.FadeInFromZero(200);
            NotificationContent.MoveToX(DrawSize.X);
            NotificationContent.MoveToX(0, 500, Easing.OutQuint);
        }

        public bool WasClosed;

        public virtual void Close()
        {
            if (WasClosed) return;

            WasClosed = true;

            Closed?.Invoke();
            this.FadeOut(100);
            Expire();
        }

        private class CloseButton : RhythmicClickableContainer
        {
            private Color4 hoverColour;

            public CloseButton()
            {
                Colour = RhythmicColors.Gray(1f).Opacity(0.5f);
                AutoSizeAxes = Axes.Both;

                Children = new[]
                {
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = FontAwesome.Solid.TimesCircle,
                        Size = new Vector2(20),
                    }
                };

                hoverColour = RhythmicColors.Yellow.Opacity(0.7f);
            }

            protected override bool OnHover(HoverEvent e)
            {
                this.FadeColour(hoverColour, 200);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                this.FadeColour(RhythmicColors.Gray(1f).Opacity(0.5f), 200);
                base.OnHoverLost(e);
            }
        }
    }
}
