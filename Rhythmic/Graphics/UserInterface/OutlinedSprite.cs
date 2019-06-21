using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

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
                //update yet again
            }
        }

        private float thickness = 10;

        public float Thickness
        {
            get => thickness;
            set
            {
                if (thickness == value) return;

                thickness = value;
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new CircularContainer
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
                    new BufferedContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.8f,
                        BlurSigma = new Vector2(blurSigma),
                        Children = new Drawable[]
                        {
                            new Sprite
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
                            Colour = Color4.Black.Opacity(1f),
                            Offset = new Vector2(0, 1.5f),
                            Type = EdgeEffectType.Shadow,
                            Radius = 10
                        },
                        Children = new Drawable[]
                        {
                            new Sprite
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