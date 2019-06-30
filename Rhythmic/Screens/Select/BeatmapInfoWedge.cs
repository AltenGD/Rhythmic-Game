using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.MathUtils;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using Rhythmic.Beatmap.Drawables;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Beatmap;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Screens.Select
{
    public class BeatmapInfoWedge : OverlayContainer
    {
        private static readonly Vector2 wedged_container_shear = new Vector2(0.15f, 0);

        protected BufferedWedgeInfo Info;

        public BeatmapInfoWedge()
        {
            Shear = wedged_container_shear;
            Masking = true;
            BorderColour = new Color4(221, 255, 255, 150);
            BorderThickness = 2.5f;
            Alpha = 0;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Glow,
                Colour = new Color4(130, 204, 255, 150),
                Radius = 20,
                Roundness = 15,
            };
        }

        protected override bool BlockPositionalInput => false;

        protected override void PopIn()
        {
            this.MoveToX(0, 800, Easing.OutQuint);
            this.RotateTo(0, 800, Easing.OutQuint);
            this.FadeIn(250);
        }

        protected override void PopOut()
        {
            this.MoveToX(-100, 800, Easing.In);
            this.RotateTo(10, 800, Easing.In);
            this.FadeOut(500, Easing.In);
        }

        private BeatmapMeta beatmap;

        public BeatmapMeta Beatmap
        {
            get => beatmap;
            set
            {
                if (beatmap == value) return;

                beatmap = value;
                updateDisplay();
            }
        }

        public override bool IsPresent => base.IsPresent || Info == null; // Visibility is updated in the LoadComponentAsync callback

        private BufferedWedgeInfo loadingInfo;

        private void updateDisplay()
        {
            void removeOldInfo()
            {
                State.Value = beatmap == null ? Visibility.Hidden : Visibility.Visible;

                Info?.FadeOut(250);
                Info?.Expire();
                Info = null;
            }

            if (beatmap == null)
            {
                removeOldInfo();
                return;
            }

            LoadComponentAsync(loadingInfo = new BufferedWedgeInfo(beatmap)
            {
                Shear = -Shear,
                Depth = Info?.Depth + 1 ?? 0
            }, loaded =>
            {
                // ensure we are the most recent loaded wedge.
                if (loaded != loadingInfo) return;

                removeOldInfo();
                Add(Info = loaded);
            });
        }

        public class BufferedWedgeInfo : BufferedContainer
        {
            public SpriteText VersionLabel { get; private set; }
            public SpriteText TitleLabel { get; private set; }
            public SpriteText ArtistLabel { get; private set; }
            public BeatmapSetOnlineStatusPill StatusPill { get; private set; }
            public FillFlowContainer MapperContainer { get; private set; }
            public FillFlowContainer InfoLabelContainer { get; private set; }

            private ILocalisedBindableString titleBinding;
            private ILocalisedBindableString artistBinding;

            private readonly BeatmapMeta beatmap;

            public BufferedWedgeInfo(BeatmapMeta beatmap)
            {
                this.beatmap = beatmap;
            }

            [BackgroundDependencyLoader]
            private void load(LocalisationManager localisation)
            {
                var metadata = beatmap.Metadata ?? new BeatmapMetadata();

                PixelSnapping = true;
                CacheDrawnFrameBuffer = true;
                RelativeSizeAxes = Axes.Both;

                titleBinding = localisation.GetLocalisedString(new LocalisedString((metadata.Song.NameUnicode, metadata.Song.Name)));
                artistBinding = localisation.GetLocalisedString(new LocalisedString((metadata.Song.AuthorUnicode, metadata.Song.Author)));

                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = ColourInfo.GradientVertical(Color4.White, Color4.White.Opacity(0.3f)),
                        Children = new[]
                        {
                            new BeatmapBackgroundSprite(beatmap)
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                FillMode = FillMode.Fill,
                            },
                        }
                    },
                    new FillFlowContainer
                    {
                        Name = "Topleft-aligned metadata",
                        Anchor = Anchor.TopLeft,
                        Origin = Anchor.TopLeft,
                        Direction = FillDirection.Horizontal,
                        Margin = new MarginPadding { Top = 10, Left = 10, Right = 10, Bottom = 20 },
                        Spacing = new Vector2(5, 0),
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new CircularContainer
                            {
                                RelativeSizeAxes = Axes.Y,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Width = 2,
                                CornerRadius = 1f,
                                Masking = true,
                                EdgeEffect = new EdgeEffectParameters
                                {
                                    Colour = RhythmicColors.FromHex("82ff05").Opacity(0.25f),
                                    Type = EdgeEffectType.Glow,
                                    Radius = 2,
                                    Roundness = 1,
                                },
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = RhythmicColors.FromHex("82ff05"),
                                    }
                                }
                            },
                            VersionLabel = new SpriteText
                            {
                                Text = "Placeholder :)",
                                Font = RhythmicFont.GetFont(size: 24, italics: true),
                            }
                        }
                    },
                    new FillFlowContainer
                    {
                        Name = "Topright-aligned metadata",
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Direction = FillDirection.Vertical,
                        Margin = new MarginPadding { Top = 14, Left = 10, Right = 18, Bottom = 20 },
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            StatusPill = new BeatmapSetOnlineStatusPill
                            {
                                TextSize = 11,
                                TextPadding = new MarginPadding { Horizontal = 8, Vertical = 2 },
                                Status = BeatmapSetOnlineStatus.Pending,
                            }
                        }
                    },
                    new FillFlowContainer
                    {
                        Name = "Centre-aligned metadata",
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.TopLeft,
                        Y = -22,
                        Direction = FillDirection.Vertical,
                        Margin = new MarginPadding { Top = 15, Left = 10, Right = 10, Bottom = 20 },
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            TitleLabel = new SpriteText
                            {
                                Font = RhythmicFont.GetFont(size: 28, italics: true)
                            },
                            ArtistLabel = new SpriteText
                            {
                                Font = RhythmicFont.GetFont(size: 17, italics: true)
                            },
                            MapperContainer = new FillFlowContainer
                            {
                                Margin = new MarginPadding { Top = 10 },
                                Direction = FillDirection.Horizontal,
                                AutoSizeAxes = Axes.Both,
                                Children = getMapper(metadata)
                            },
                            InfoLabelContainer = new FillFlowContainer
                            {
                                Margin = new MarginPadding { Top = 20 },
                                Spacing = new Vector2(20, 0),
                                AutoSizeAxes = Axes.Both,
                                Children = getInfoLabels()
                            }
                        }
                    }
                };

                titleBinding.BindValueChanged(_ => setMetadata());
                artistBinding.BindValueChanged(_ => setMetadata(), true);
            }

            private void setMetadata()
            {
                ArtistLabel.Text = artistBinding.Value;
                TitleLabel.Text = titleBinding.Value;
                ForceRedraw();
            }

            private InfoLabel[] getInfoLabels()
            {
                var b = beatmap.Level;

                List<InfoLabel> labels = new List<InfoLabel>();

                if (b?.Level?.Any() == true)
                {
                    Object lastObject = b.Level.LastOrDefault();
                    double endTime = lastObject?.TotalTime + lastObject?.Time ?? 0;

                    labels.Add(new InfoLabel(new BeatmapStatistic
                    {
                        Name = "Length",
                        Icon = FontAwesome.Regular.Clock,
                        Content = TimeSpan.FromMilliseconds(endTime - b.Level.First().Time).ToString(@"m\:ss")
                    }));

                    labels.Add(new InfoLabel(new BeatmapStatistic
                    {
                        Name = "BPM",
                        Icon = FontAwesome.Regular.Circle,
                        Content = beatmap.Metadata.Level.BPM.ToString()
                    }));

                    labels.Add(new InfoLabel(new BeatmapStatistic
                    {
                        Name = "Objects",
                        Icon = FontAwesome.Solid.Square,
                        Content = b.Level.Count.ToString()
                    }));
                }

                return labels.ToArray();
            }

            private SpriteText[] getMapper(BeatmapMetadata metadata)
            {
                if (string.IsNullOrEmpty(metadata.Level?.CreatorName))
                    return Array.Empty<SpriteText>();

                return new[]
                {
                    new SpriteText
                    {
                        Text = "mapped by ",
                        Font = RhythmicFont.GetFont(size: 15)
                    },
                    new SpriteText
                    {
                        Text = metadata.Level.CreatorName,
                        Font = RhythmicFont.GetFont(weight: FontWeight.SemiBold, size: 15)
                    }
                };
            }
        }

        public class InfoLabel : Container, IHasTooltip
        {
            public string TooltipText { get; private set; }

            public InfoLabel(BeatmapStatistic statistic)
            {
                TooltipText = statistic.Name;
                AutoSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(20),
                        Children = new[]
                        {
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Colour = RhythmicColors.FromHex(@"441288"),
                                Icon = FontAwesome.Solid.Square,
                                Rotation = 45,
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Scale = new Vector2(0.8f),
                                Colour = RhythmicColors.FromHex(@"f7dd55"),
                                Icon = statistic.Icon,
                            },
                        }
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Colour = new Color4(255, 221, 85, 255),
                        Font = RhythmicFont.GetFont(Typeface.Digitall, 17),
                        Margin = new MarginPadding { Left = 30 },
                        Text = statistic.Content,
                    }
                };
            }
        }
    }
}
