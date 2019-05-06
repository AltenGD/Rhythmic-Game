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

namespace Rhythmic.Graphics.Drawables
{
    public class RhythmicRectangleOutline : SmoothPath
    {
        public delegate List<Vector2> ApproximatorFunc(ReadOnlySpan<Vector2> controlPoints);
        public float widthMin = 134, widthMax = 972;

        public RhythmicRectangleOutline(ApproximatorFunc approximator)
        {
            Vector2[] points = new Vector2[7];
            points[0] = new Vector2(widthMin, 0);
            points[1] = new Vector2(widthMax, 0);
            points[2] = new Vector2(1106, 134.5f);
            points[3] = new Vector2(widthMax, 268);
            points[4] = new Vector2(widthMin, 268);
            points[5] = new Vector2(0, 133.5f);
            points[6] = new Vector2(widthMin, 0);

            PathRadius = 2;
            Vertices = approximator(points);
        }
    }
}
