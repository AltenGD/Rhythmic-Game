using System;
using System.Linq;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.UserInterface
{
    public class RhythmicTabControl : TabControl<string>
    {
        private readonly Box bar;

        private Color4 accentColour = Color4.White;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                if (accentColour == value)
                    return;

                accentColour = value;
                bar.Colour = value;

                foreach (TabItem<string> tabItem in TabContainer)
                {
                    ((RhythmicTabItem)tabItem).AccentColour = value;
                }
            }
        }

        public new MarginPadding Padding
        {
            get => TabContainer.Padding;
            set => TabContainer.Padding = value;
        }

        public RhythmicTabControl()
        {
            TabContainer.Masking = false;
            TabContainer.Spacing = new Vector2(15, 0);

            AddInternal(bar = new Box
            {
                RelativeSizeAxes = Axes.X,
                Height = 2,
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.CentreLeft
            });
        }

        protected override Dropdown<string> CreateDropdown() => null;

        protected override TabItem<string> CreateTabItem(string value) => new RhythmicTabItem(value)
        {
            AccentColour = AccentColour
        };

        private class RhythmicTabItem : TabItem<string>
        {
            private readonly SpriteText text;
            private readonly ExpandingBar bar;

            private Color4 accentColour;

            public Color4 AccentColour
            {
                get => accentColour;
                set
                {
                    if (accentColour == value)
                        return;

                    accentColour = value;
                    bar.Colour = value;

                    updateState();
                }
            }

            public RhythmicTabItem(string value)
                : base(value)
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;

                Children = new Drawable[]
                {
                    text = new SpriteText
                    {
                        Margin = new MarginPadding { Bottom = 10 },
                        Origin = Anchor.BottomLeft,
                        Anchor = Anchor.BottomLeft,
                        Text = value,
                    },
                    bar = new ExpandingBar
                    {
                        Anchor = Anchor.BottomCentre,
                        ExpandedSize = 7.5f,
                        CollapsedSize = 0
                    },
                };
            }

            protected override void OnActivated() => updateState();

            protected override void OnDeactivated() => updateState();

            private void updateState()
            {
                if (Active.Value)
                {
                    text.FadeColour(Color4.White, 120, Easing.InQuad);
                    bar.Expand();

                    if (Active.Value)
                        text.Font = text.Font = RhythmicFont.GetFont(size: 25, weight: FontWeight.Bold);
                }
                else
                {
                    text.FadeColour(AccentColour, 120, Easing.InQuad);
                    bar.Collapse();
                    text.Font = text.Font = RhythmicFont.GetFont(size: 25, weight: FontWeight.Regular);
                }
            }
        }
    }
}
