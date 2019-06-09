using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap.Drawables.Components;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Sprites;
using System.Collections.Generic;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapCard : ClickableContainer, IHasFilterTerms
    {
        private Vector2 size = new Vector2(330, 150);

        public BeatmapMeta Beatmap;

        public IEnumerable<string> FilterTerms => new[]
        {
            Beatmap.CreatedAt.ToString(),
            Beatmap.DownloadedAt.ToString(),
            Beatmap.Metadata.Level.LevelName,
            Beatmap.Metadata.Level.CreatorName,
            Beatmap.Metadata.Song.Name,
            Beatmap.Metadata.Song.Author,
        };

        public BeatmapCard(BeatmapMeta beatmap)
        {
            Beatmap = beatmap;
            Size = size;
            CornerRadius = 5;
            Masking = true;

            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Offset = new Vector2(0f, 2f),
                Radius = 1f,
                Colour = Color4.Black.Opacity(0.25f),
            };

            Children = new Drawable[]
{
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                new BeatmapBackgroundSprite(beatmap)
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0.6f,
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new ColourInfo
                    {
                        TopLeft = Color4.Black.Opacity(0f),
                        TopRight = Color4.Black.Opacity(0f),
                        BottomLeft = Color4.Black.Opacity(0.6f),
                        BottomRight = Color4.Black.Opacity(0.6f)
                    }
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Padding = new MarginPadding
                    {
                        Bottom = size.Y / 4 + 5,
                        Left = 5
                    },
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = beatmap.Metadata.Song.Author,
                           Font = RhythmicFont.GetFont(size: 20, weight: FontWeight.Medium),
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft
                        },
                        new SpriteText
                        {
                            Text = beatmap.Metadata.Song.Name,
                            Font = RhythmicFont.GetFont(size: 30, weight: FontWeight.Medium),
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft
                        }
                    }
                },
                new BeatmapUsecase(beatmap)
                {
                    Origin = Anchor.BottomLeft,
                    Anchor = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1f, size.Y / 4)
                }
            };
        }
    }
}
