using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using System.Collections.Generic;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Tests.Visual
{
    public class TestCaseRhythmicRectangle : TestCase
    {
        private RhythmicRectangleOutline rec;

        public TestCaseRhythmicRectangle()
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
                    AccentColor = RhythmicColors.HotPink
                }
            });
        }
    }
}