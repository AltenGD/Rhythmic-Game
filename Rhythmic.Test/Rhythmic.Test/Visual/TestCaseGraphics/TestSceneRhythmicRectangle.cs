using osu.Framework.Graphics;
using osu.Framework.Testing;
using osuTK;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Tests.Visual
{
    public class TestSceneRhythmicRectangle : TestScene
    {
        private RhythmicRectangleOutline rec;

        public TestSceneRhythmicRectangle()
        {
            AddRange(new Drawable[]
            {
                new RhythmicRectangleFill
                {
                    Scale = new Vector2(0.4f),
                    FillColor = RhythmicColors.GrayF
                },
                rec = new RhythmicRectangleOutline(PathApproximator.ApproximateLinear)
                {
                    PathRadius = 10,
                    Scale = new Vector2(0.4f),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = RhythmicColors.HotPink
                }
            });
        }
    }
}