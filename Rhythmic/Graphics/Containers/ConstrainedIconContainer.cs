using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osuTK;

namespace Rhythmic.Graphics.Containers
{
    /// <summary>Display an icon that is forced to scale to the size of this container. </summary>
    public class ConstrainedIconContainer : CompositeDrawable
    {
        public Drawable Icon
        {
            get => InternalChild;

            set => InternalChild = value;
        }

        /// <summary>Determines an edge effect of this <see cref="Container"/>.
        /// Edge effects are e.g. glow or a shadow.
        /// Only has an effect when <see cref="CompositeDrawable.Masking"/> is true.</summary>
        public new EdgeEffectParameters EdgeEffect
        {
            get => base.EdgeEffect;
            set => base.EdgeEffect = value;
        }

        protected override void Update()
        {
            base.Update();

            if (InternalChildren.Count > 0 && InternalChild.DrawSize.X > 0)
            {
                var fitScale = Math.Min(DrawSize.X / InternalChild.DrawSize.X, DrawSize.Y / InternalChild.DrawSize.Y);
                InternalChild.Scale = new Vector2(fitScale);
                InternalChild.Anchor = Anchor.Centre;
                InternalChild.Origin = Anchor.Centre;
            }
        }

        public ConstrainedIconContainer()
        {
            Masking = true;
        }
    }
}
