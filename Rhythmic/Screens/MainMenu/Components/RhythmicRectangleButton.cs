using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

namespace Rhythmic.Screens.MainMenu.Components
{
    public class RhythmicRectangleButton : Container
    {
        private RhythmicRectangleOutline outline;
        private Color4 Hover;
        private Color4 Normal;
        private Action action;
        private RhythmicRectangleFill shadow;

        public RhythmicRectangleButton(string Name, IconUsage Icon, Color4 AccentColor, Color4 AccentColorBrighter, Action Action)
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Hover = AccentColorBrighter;
            Normal = AccentColor;
            action = Action;

            Children = new Drawable[]
            {
                shadow = new RhythmicRectangleFill
                {
                    FillColor = Color4.Black.Opacity(0.5f),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.3f),
                },
                new RhythmicRectangleFill
                {
                    FillColor = Color4.White,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Scale = new Vector2(0.3f),
                },
                outline = new RhythmicRectangleOutline(PathApproximator.ApproximateLinear)
                {
                    PathRadius = 10,
                    Scale = new Vector2(0.3f),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = AccentColor
                },
                new FillFlowContainer
                {
                    Scale = new Vector2(0.3f),
                    Size = new Vector2(1126, 288),
                    Direction = FillDirection.Horizontal,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Spacing = new Vector2(40),
                    Padding = new MarginPadding
                    {
                        Horizontal = 180
                    },
                    Children = new Drawable[]
                    {
                        new SpriteIcon
                        {
                            Shadow = true,
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.CentreLeft,
                            Size = new Vector2(130),
                            Colour = AccentColor,
                            Icon = Icon
                        },
                        new SpriteText
                        {
                            Colour = AccentColor,
                            Text = Name,
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.CentreLeft,
                            Font = RhythmicFont.GetFont(size:160)
                        }
                    }
                }
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            shadow.MoveTo(new Vector2(0, 5), 500, Easing.OutExpo);
            outline.FadeColour(Hover, 500, Easing.OutExpo);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            shadow.MoveTo(new Vector2(0, 0), 500, Easing.OutExpo);
            outline.FadeColour(Normal, 500, Easing.OutExpo);
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            action?.Invoke();
            return base.OnClick(e);
        }
    }
}
