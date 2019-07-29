using osu.Framework.Allocation;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Graphics.UserInterface
{
    public class RhythmicMenu : Menu
    {
        public RhythmicMenu(Direction direction, bool topLevelMenu = false)
            : base(direction, topLevelMenu)
        {
            BackgroundColour = Color4.Black.Opacity(0.5f);

            MaskingContainer.CornerRadius = 4;
            ItemsContainer.Padding = new MarginPadding(5);
        }

        protected override void AnimateOpen() => this.FadeIn(300, Easing.OutQuint);
        protected override void AnimateClose() => this.FadeOut(300, Easing.OutQuint);

        protected override void UpdateSize(Vector2 newSize)
        {
            if (Direction == Direction.Vertical)
            {
                Width = newSize.X;
                this.ResizeHeightTo(newSize.Y, 300, Easing.OutQuint);
            }
            else
            {
                Height = newSize.Y;
                this.ResizeWidthTo(newSize.X, 300, Easing.OutQuint);
            }
        }

        protected override DrawableMenuItem CreateDrawableMenuItem(MenuItem item) => new DrawableRhythmicMenuItem(item);

        protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new RhythmicScrollContainer(direction);

        protected override Menu CreateSubMenu() => new RhythmicMenu(Direction.Vertical)
        {
            Anchor = Direction == Direction.Horizontal ? Anchor.BottomLeft : Anchor.TopRight
        };

        protected class DrawableRhythmicMenuItem : DrawableMenuItem
        {
            private const int margin_horizontal = 17;
            private const int text_size = 17;
            private const int transition_length = 80;
            public const int MARGIN_VERTICAL = 4;

            private TextContainer text;

            public DrawableRhythmicMenuItem(MenuItem item)
                : base(item)
            {
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                BackgroundColour = Color4.Transparent;
                BackgroundColourHover = RhythmicColors.FromHex(@"172023");

                updateTextColour();
            }

            private void updateTextColour()
            {
                switch ((Item as RhythmicMenuItem)?.Type)
                {
                    default:
                    case MenuItemType.Standard:
                        text.Colour = Color4.White;
                        break;

                    case MenuItemType.Destructive:
                        text.Colour = Color4.Red;
                        break;

                    case MenuItemType.Highlighted:
                        text.Colour = RhythmicColors.FromHex(@"ffcc22");
                        break;
                }
            }

            protected override bool OnHover(HoverEvent e)
            {
                text.BoldText.FadeIn(transition_length, Easing.OutQuint);
                text.NormalText.FadeOut(transition_length, Easing.OutQuint);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                text.BoldText.FadeOut(transition_length, Easing.OutQuint);
                text.NormalText.FadeIn(transition_length, Easing.OutQuint);
                base.OnHoverLost(e);
            }

            protected sealed override Drawable CreateContent() => text = CreateTextContainer();
            protected virtual TextContainer CreateTextContainer() => new TextContainer();

            protected class TextContainer : Container, IHasText
            {
                public string Text
                {
                    get => NormalText.Text;
                    set
                    {
                        NormalText.Text = value;
                        BoldText.Text = value;
                    }
                }

                public readonly SpriteText NormalText;
                public readonly SpriteText BoldText;

                public TextContainer()
                {
                    Anchor = Anchor.CentreLeft;
                    Origin = Anchor.CentreLeft;

                    AutoSizeAxes = Axes.Both;

                    Children = new Drawable[]
                    {
                        NormalText = new SpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Font = RhythmicFont.GetFont(size: text_size),
                            Margin = new MarginPadding { Horizontal = margin_horizontal, Vertical = MARGIN_VERTICAL },
                        },
                        BoldText = new SpriteText
                        {
                            AlwaysPresent = true,
                            Alpha = 0,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Font = RhythmicFont.GetFont(size: text_size, weight: FontWeight.SemiBold),
                            Margin = new MarginPadding { Horizontal = margin_horizontal, Vertical = MARGIN_VERTICAL },
                        }
                    };
                }
            }
        }
    }
}