using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap.Drawables;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhythmic.Overlays.Music
{
    public class PlaylistItem : Container, IFilterable, IDraggable
    {
        private const float fade_duration = 100;

        private Color4 hoverColour;
        private Color4 artistColour;

        private SpriteIcon handle;
        private TextFlowContainer text;
        private IEnumerable<Drawable> titleSprites;
        private ILocalisedBindableString titleBind;
        private ILocalisedBindableString artistBind;

        public readonly BeatmapMeta Beatmap;

        public Action<BeatmapMeta> OnSelect;

        public bool IsDraggable { get; private set; }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            IsDraggable = handle.IsHovered;
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            IsDraggable = false;
            return base.OnMouseUp(e);
        }

        private bool selected;

        public bool Selected
        {
            get => selected;
            set
            {
                if (value == selected) return;

                selected = value;

                FinishTransforms(true);
                foreach (Drawable s in titleSprites)
                    s.FadeColour(Selected ? hoverColour : Color4.White, fade_duration);
            }
        }

        public PlaylistItem(BeatmapMeta setInfo)
        {
            Beatmap = setInfo;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Top = 3, Bottom = 3 };
        }

        [BackgroundDependencyLoader]
        private void load(LocalisationManager localisation)
        {
            hoverColour = RhythmicColors.Yellow;
            artistColour = Color4.White.Opacity(0.8f);

            SongMetadata metadata = Beatmap.Metadata.Song;

            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new BeatmapBackgroundSprite(Beatmap)
                        {
                            Colour = ColourInfo.GradientHorizontal(Color4.White.Opacity(0.5f), Color4.White.Opacity(0f)),
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fill,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        }
                    }
                },
                handle = new PlaylistItemHandle
                {
                    Colour = Color4.White.Opacity(0.8f)
                },
                text = new TextFlowContainer
                {
                    Padding = new MarginPadding { Left = 30, Vertical = 10 },
                    Direction = FillDirection.Full,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ContentIndent = 10f,
                }
            };

            titleBind = localisation.GetLocalisedString(new LocalisedString((metadata.NameUnicode, metadata.Name)));
            artistBind = localisation.GetLocalisedString(new LocalisedString((metadata.AuthorUnicode, metadata.Author)));

            artistBind.BindValueChanged(_ => recreateText(), true);
        }

        private void recreateText()
        {
            text.Clear();

            //space after the title to put a space between the title and artist
            titleSprites = text.AddText(titleBind.Value + @"  ", sprite => sprite.Font = RhythmicFont.GetFont(size: 20, weight: FontWeight.Bold)).OfType<SpriteText>();

            text.AddText(artistBind.Value, sprite =>
            {
                sprite.Font = RhythmicFont.GetFont(size: 18);
                sprite.Colour = artistColour;
                sprite.Padding = new MarginPadding { Top = 1 };
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            handle.FadeIn(fade_duration);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            handle.FadeOut(fade_duration);
        }

        protected override bool OnClick(ClickEvent e)
        {
            OnSelect?.Invoke(Beatmap);
            return true;
        }

        public IEnumerable<string> FilterTerms { get; private set; }

        private bool matching = true;

        public bool MatchingFilter
        {
            get => matching;
            set
            {
                if (matching == value) return;

                matching = value;

                this.FadeTo(matching ? 1 : 0, 200);
            }
        }

        public bool FilteringActive { get; set; }

        private class PlaylistItemHandle : SpriteIcon
        {
            public PlaylistItemHandle()
            {
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;
                Size = new Vector2(12);
                Icon = FontAwesome.Solid.Bars;
                Alpha = 0f;
                Margin = new MarginPadding { Left = 5, Top = 2 };
            }

            public override bool HandlePositionalInput => IsPresent;
        }
    }

    public interface IDraggable : IDrawable
    {
        /// <summary>Whether this <see cref="IDraggable"/> can be dragged in its current state.</summary>
        bool IsDraggable { get; }
    }
}
