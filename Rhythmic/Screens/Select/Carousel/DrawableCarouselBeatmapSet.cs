using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Drawables;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Screens.Select.Carousel
{
    public class DrawableCarouselBeatmapSet : DrawableCarouselItem
    {
        private readonly BeatmapMeta beatmapSet;

        public DrawableCarouselBeatmapSet(CarouselBeatmapSet set)
            : base(set)
        {
            beatmapSet = set.BeatmapSet;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new DelayedLoadUnloadWrapper(() =>
                    {
                        PanelBackground background = new PanelBackground(beatmapSet)
                        {
                            RelativeSizeAxes = Axes.Both
                        };

                        background.OnLoadComplete += d => d.FadeInFromZero(1000, Easing.OutQuint);

                        return background;
                    }, 300, 5000
                ),
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    Padding = new MarginPadding { Top = 10, Left = 18, Right = 10, Bottom = 10 },
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Spacing = new Vector2(10, 0),
                    Children = new Drawable[]
                    {
                        new OutlinedSprite
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Texture = beatmapSet.Logo ?? beatmapSet.Background,
                            Size = new Vector2(70),
                            BlurSigma = 10,
                            Thickness = 5f
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = new LocalisedString((beatmapSet.Metadata.Song.NameUnicode, beatmapSet.Metadata.Song.Name)),
                                    Font = RhythmicFont.GetFont(weight: FontWeight.SemiBold, size: 22, italics: true),
                                    Shadow = true,
                                },
                                new SpriteText
                                {
                                    Text = new LocalisedString((beatmapSet.Metadata.Song.AuthorUnicode, beatmapSet.Metadata.Song.Author)),
                                    Font = RhythmicFont.GetFont(weight: FontWeight.Regular, size: 17, italics: true),
                                    Shadow = true,
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    AutoSizeAxes = Axes.Both,
                                    Margin = new MarginPadding { Top = 5 },
                                    Spacing = new Vector2(5, 0),
                                    Children = new Drawable[]
                                    {
                                        new BeatmapSetOnlineStatusPill
                                        {
                                            Origin = Anchor.CentreLeft,
                                            Anchor = Anchor.CentreLeft,
                                            Margin = new MarginPadding { Right = 5 },
                                            TextSize = 16,
                                            TextPadding = new MarginPadding { Horizontal = 8, Vertical = 2 },
                                            Status = BeatmapSetOnlineStatus.Pending
                                        },
                                        new Container
                                        {
                                            AutoSizeAxes = Axes.Both,
                                            Masking = true,
                                            CornerRadius = 5,
                                            Children = new Drawable[]
                                            {
                                                new Box
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Colour = Color4.Black,
                                                    Alpha = 0.5f,
                                                },
                                                new FillFlowContainer
                                                {
                                                    AutoSizeAxes = Axes.Both,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Direction = FillDirection.Vertical,
                                                    Padding = new MarginPadding
                                                    {
                                                        Horizontal = 10,
                                                    },
                                                    Children = new Drawable[]
                                                    {
                                                        new CircularContainer
                                                        {
                                                            Position = new Vector2(0, 2),
                                                            RelativeSizeAxes = Axes.X,
                                                            Anchor = Anchor.TopCentre,
                                                            Origin = Anchor.TopCentre,
                                                            Width = 2,
                                                            Height = 2,
                                                            CornerRadius = 1f,
                                                            Masking = true,
                                                            EdgeEffect = new EdgeEffectParameters
                                                            {
                                                                Colour = RhythmicColors.FromHex("9CB913").Opacity(0.25f),
                                                                Type = EdgeEffectType.Glow,
                                                                Radius = 2,
                                                                Roundness = 1,
                                                            },
                                                            Children = new Drawable[]
                                                            {
                                                                new Box
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Colour = RhythmicColors.FromHex("9CB913"),
                                                                }
                                                            }
                                                        },
                                                        new SpriteText
                                                        {
                                                            Text = "1",
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private class PanelBackground : BufferedContainer
        {
            public PanelBackground(BeatmapMeta working)
            {
                CacheDrawnFrameBuffer = true;

                Children = new Drawable[]
                {
                    new BufferedContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        BlurSigma = new Vector2(15),
                        Children = new Drawable[]
                        {
                            new BeatmapBackgroundSprite(working)
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                FillMode = FillMode.Fill,
                            },
                        }
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(0.9f),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Masking = true,
                        Padding = new MarginPadding
                        {
                            Left = 5
                        },
                        Children = new Drawable[]
                        {
                            new BeatmapBackgroundSprite(working)
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                FillMode = FillMode.Fill,
                            },
                        }
                    },
                    // Todo: This should be a fill flow, but has invalidation issues (see https://github.com/ppy/osu-framework/issues/223)
                    new Container
                    {
                        Depth = -1,
                        RelativeSizeAxes = Axes.Both,
                        // This makes the gradient not be perfectly horizontal, but diagonal at a ~40° angle
                        Shear = new Vector2(0.8f, 0),
                        Alpha = 0.5f,
                        Children = new[]
                        {
                            // The left half with no gradient applied
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Colour = Color4.Black,
                                Width = 0.4f,
                            },
                            // Piecewise-linear gradient with 3 segments to make it appear smoother
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Colour = ColourInfo.GradientHorizontal(Color4.Black, new Color4(0f, 0f, 0f, 0.9f)),
                                Width = 0.05f,
                                X = 0.4f,
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Colour = ColourInfo.GradientHorizontal(new Color4(0f, 0f, 0f, 0.9f), new Color4(0f, 0f, 0f, 0.1f)),
                                Width = 0.2f,
                                X = 0.45f,
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Colour = ColourInfo.GradientHorizontal(new Color4(0f, 0f, 0f, 0.1f), new Color4(0, 0, 0, 0)),
                                Width = 0.05f,
                                X = 0.65f,
                            },
                        }
                    },
                };
            }
        }
    }
}
