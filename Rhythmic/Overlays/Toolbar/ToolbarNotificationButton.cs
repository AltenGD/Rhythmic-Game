using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Overlays.Toolbar
{
    public class ToolbarNotificationButton : ToolbarOverlayToggleButton
    {
        protected override Anchor TooltipAnchor => Anchor.TopRight;

        public BindableInt NotificationCount = new BindableInt();

        private readonly NotificationCircle countDisplay;

        public ToolbarNotificationButton()
        {
            Icon = FontAwesome.Solid.Bars;
            TooltipMain = "Notifications";
            TooltipSub = "Waiting for 'ya";

            Add(countDisplay = new NotificationCircle
            {
                Alpha = 0,
                Width = 5,
                Height = 5,
                Origin = Anchor.BottomCentre,
                Anchor = Anchor.BottomCentre,
                Position = new Vector2(0, -2)
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load(NotificationOverlay notificationOverlay)
        {
            StateContainer = notificationOverlay;

            if (notificationOverlay != null)
                NotificationCount.BindTo(notificationOverlay.UnreadCount);

            NotificationCount.ValueChanged += count =>
            {
                if (count.NewValue == 0)
                    countDisplay.FadeOut(200, Easing.OutQuint);
                else
                {
                    countDisplay.Count = count.NewValue;
                    countDisplay.FadeIn(200, Easing.OutQuint);
                }
            };
        }

        private class NotificationCircle : CompositeDrawable
        {
            private readonly Circle pulsatingCircle;
            private readonly Circle circle;

            private int count;

            public int Count
            {
                get => count;
                set
                {
                    if (count == value)
                        return;

                    if (value > count)
                    {
                        circle.FlashColour(Color4.White, 600, Easing.OutQuint);
                        this.ScaleTo(1.1f).Then().ScaleTo(1, 600, Easing.OutElastic);
                    }

                    count = value;
                }
            }

            public NotificationCircle()
            {
                InternalChildren = new Drawable[]
                {
                    circle = new Circle
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = RhythmicColors.Red
                    },
                    pulsatingCircle = new Circle
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Y = -1,
                        Scale = new Vector2(1),
                        Colour = RhythmicColors.Red,
                        Alpha = 0
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                pulsatingCircle.FadeIn().ScaleTo(1.1f, 500, Easing.OutExpo).FadeOut(500).Delay(500).ScaleTo(1f).Loop();
            }
        }
    }
}
