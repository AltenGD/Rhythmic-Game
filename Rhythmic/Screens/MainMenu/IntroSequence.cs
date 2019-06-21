using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using System.Linq;

namespace Rhythmic.Screens.MainMenu
{
    public class IntroSequence : Container
    {
        private const float logo_size = 290;

        private SpriteText welcomeText;

        private Container<Box> lines;

        private Box lineTopLeft;
        private Box lineBottomLeft;
        private Box lineTopRight;
        private Box lineBottomRight;

        private Ring smallRing;
        private Ring mediumRing;
        private Ring bigRing;

        private Box backgroundFill;
        private Box foregroundFill;

        public IntroSequence()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            const int line_offset = 80;

            Rotation = -45;

            Children = new Drawable[]
            {
                lines = new Container<Box>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        lineTopLeft = new Box
                        {
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.Centre,
                            Position = new Vector2(-line_offset, -line_offset),
                            Rotation = 45,
                            Colour = Color4.White.Opacity(180),
                        },
                        lineTopRight = new Box
                        {
                            Origin = Anchor.CentreRight,
                            Anchor = Anchor.Centre,
                            Position = new Vector2(line_offset, -line_offset),
                            Rotation = -45,
                            Colour = Color4.White.Opacity(80),
                        },
                        lineBottomLeft = new Box
                        {
                            Origin = Anchor.CentreLeft,
                            Anchor = Anchor.Centre,
                            Position = new Vector2(-line_offset, line_offset),
                            Rotation = -45,
                            Colour = Color4.White.Opacity(230),
                        },
                        lineBottomRight = new Box
                        {
                            Origin = Anchor.CentreRight,
                            Anchor = Anchor.Centre,
                            Position = new Vector2(line_offset, line_offset),
                            Rotation = 45,
                            Colour = Color4.White.Opacity(130),
                        },
                    }
                },
                bigRing = new Ring(RhythmicColors.FromHex(@"B6C5E9"), 0.85f),
                mediumRing = new Ring(Color4.White.Opacity(130), 0.7f),
                smallRing = new Ring(Color4.White, 0.6f),
                welcomeText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "welcome",
                    Padding = new MarginPadding { Bottom = 10 },
                    Font = RhythmicFont.GetFont(size: 74),
                    Alpha = 0,
                    Spacing = new Vector2(5),
                },
                new CircularContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(logo_size),
                    Masking = true,
                    Children = new Drawable[]
                    {
                        backgroundFill = new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Height = 0,
                            Colour = RhythmicColors.FromHex(@"C6D8FF").Opacity(160),
                        },
                        foregroundFill = new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = Vector2.Zero,
                            RelativeSizeAxes = Axes.Both,
                            Width = 0,
                            Colour = Color4.White,
                        },
                    }
                }
            };

            foreach (Box line in lines)
            {
                line.Size = new Vector2(105, 1.5f);
                line.Alpha = 0;
            }

            Scale = new Vector2(0.5f);
        }

        public void Start(double length)
        {
            if (Children.Any())
            {
                // restart if we were already run previously.
                FinishTransforms(true);
                load();
            }

            smallRing.ResizeTo(logo_size * 0.086f, 400, Easing.InOutQuint);

            mediumRing.ResizeTo(130, 340, Easing.OutQuad);
            mediumRing.Foreground.ResizeTo(1, 880, Easing.Out);

            double remainingTime() => length - TransformDelay;

            using (BeginDelayedSequence(250, true))
            {
                welcomeText.FadeIn(700);
                welcomeText.TransformSpacingTo(new Vector2(20, 0), remainingTime(), Easing.Out);

                const int line_duration = 700;
                const int line_resize = 150;

                foreach (Box line in lines)
                {
                    line.FadeIn(40).ResizeWidthTo(0, line_duration - line_resize, Easing.OutQuint);
                }

                const int line_end_offset = 120;

                smallRing.Foreground.ResizeTo(1, line_duration, Easing.OutQuint);

                lineTopLeft.MoveTo(new Vector2(-line_end_offset, -line_end_offset), line_duration, Easing.OutQuint);
                lineTopRight.MoveTo(new Vector2(line_end_offset, -line_end_offset), line_duration, Easing.OutQuint);
                lineBottomLeft.MoveTo(new Vector2(-line_end_offset, line_end_offset), line_duration, Easing.OutQuint);
                lineBottomRight.MoveTo(new Vector2(line_end_offset, line_end_offset), line_duration, Easing.OutQuint);
            }
        }

        private class Ring : Container<Circle>
        {
            public readonly Circle Foreground;

            public Ring(Color4 ringColour, float foregroundSize)
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                Children = new[]
                {
                    new Circle
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(0.98f),
                        Colour = ringColour,
                    },
                    Foreground = new Circle
                    {
                        Size = new Vector2(foregroundSize),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                    }
                };
            }
        }
    }
}
