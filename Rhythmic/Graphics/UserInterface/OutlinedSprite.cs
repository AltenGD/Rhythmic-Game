using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using System;

namespace Rhythmic.Graphics.UserInterface
{
    public class OutlinedSprite : CompositeDrawable
    {
        private Texture texture;

        ///<summary>Sets and updates the texture of the sprite</summary>
        public Texture Texture
        {
            get => texture;
            set
            {
                if (texture == value) return;

                texture = value;

                blurredSprite.Texture = value;
                sprite.Texture = value;
            }
        }

        private float blurSigma = 10;

        public float BlurSigma
        {
            get => blurSigma;
            set
            {
                if (blurSigma == value) return;

                blurSigma = value;
                blurContainer.BlurSigma = new Vector2(value);
            }
        }

        private float thickness = 10;

        private readonly Sprite blurredSprite;
        private readonly Sprite sprite;
        private readonly BufferedContainer blurContainer;
        private readonly CircularContainer borderContainer;

        public float Thickness
        {
            get => thickness;
            set
            {
                if (thickness == value) return;

                thickness = value;
                borderContainer.BorderThickness = value;
            }
        }

        public OutlinedSprite()
        {
            AddInternal(borderContainer = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                BorderColour = Color4.White,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                BorderThickness = thickness,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black
                    },
                    blurContainer = new BufferedContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.8f,
                        BlurSigma = new Vector2(blurSigma),
                        Children = new Drawable[]
                        {
                            blurredSprite = new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fill,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = texture,
                            }
                        }
                    },
                    new CircularContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(0.7f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Masking = true,
                        EdgeEffect = new EdgeEffectParameters
                        {
                            Roundness = Math.Min(DrawSize.X, DrawSize.Y) / 2f, //Gives it a perfectly rounded circle
                            Colour = Color4.Black.Opacity(0.25f),
                            Offset = new Vector2(0, 1.5f),
                            Type = EdgeEffectType.Shadow,
                            Radius = 10
                        },
                        Children = new Drawable[]
                        {
                            sprite = new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                FillMode = FillMode.Fill,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = texture
                            }
                        }
                    }
                }
            });
        }
    }
}