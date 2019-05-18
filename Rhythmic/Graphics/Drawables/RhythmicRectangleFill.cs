using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Rhythmic.Graphics.Drawables
{
    public class RhythmicRectangleFill : Container
    {
        public Color4 FillColor;

        private Box center, left, right;

        public RhythmicRectangleFill()
        {
            Size = new Vector2(1126, 288);
            Masking = true;
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Children = new Drawable[]
            {
                center = new Box
                {
                    Size = new Vector2(838, 288),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                },
                left = new Box
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.CentreLeft,
                    Size = new Vector2(202),
                    Rotation = 45,
                    Position = new Vector2(142, 0)
                },
                right = new Box
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.CentreRight,
                    Size = new Vector2(202),
                    Rotation = 45,
                    Position = new Vector2(-142, 0)
                }
            };
        }

        protected override void LoadComplete()
        {
            center.Colour = FillColor;
            right.Colour = FillColor;
            left.Colour = FillColor;
            base.LoadComplete();
        }
    }
}
