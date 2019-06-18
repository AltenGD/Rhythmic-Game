using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osu.Framework.Extensions.Color4Extensions;
using Rhythmic.Database;
using osu.Framework.Graphics.Colour;
using Rhythmic.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace Rhythmic.Screens.MainMenu.Components
{
    public class MenuButton : ClickableContainer
    {
        private string text = string.Empty;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                spriteText.Text = value;
            }
        }

        private IconUsage icon;

        public IconUsage Icon
        {
            get => icon;
            set
            {
                icon = value;
                spriteIcon.Icon = value;
            }
        }

        private SpriteText spriteText;
        private SpriteIcon spriteIcon;
        private Box background;

        public MenuButton()
        {
            Height = 50;
            RelativeSizeAxes = Axes.X;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Spacing = new Vector2(20, 0),
                    Padding = new MarginPadding
                    {
                        Horizontal = 20
                    },
                    Children = new Drawable[]
                    {
                        spriteIcon = new SpriteIcon
                        {
                            Size = new Vector2(25),
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Icon = icon
                        },
                        spriteText = new SpriteText
                        {
                            RelativeSizeAxes = Axes.Both,
                            Font = RhythmicFont.Default,
                            Text = text,
                        }
                    }
                }
            };
        }

        [Resolved]
        private RhythmicStore store { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            background.Colour = ColourInfo.GradientHorizontal(store.SecondaryColour.Value.Opacity(0.4f), store.SecondaryColour.Value.Opacity(0.05f));
        }

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(ColourInfo.GradientHorizontal(store.SecondaryColour.Value.Opacity(0.75f), store.SecondaryColour.Value.Opacity(0.05f)), 500, Easing.OutExpo);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e); background.FadeColour(ColourInfo.GradientHorizontal(store.SecondaryColour.Value.Opacity(0.4f), store.SecondaryColour.Value.Opacity(0.05f)), 500, Easing.OutExpo);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            this.ResizeHeightTo(40, 1000, Easing.InOutQuad);
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            this.ResizeHeightTo(50, 500, Easing.OutElastic);
            return base.OnMouseUp(e);
        }
    }
}