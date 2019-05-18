using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Test.Visual
{
    public class TestSceneFont : TestScene
    {
        private FontWeight Weight = FontWeight.Regular;
        private Typeface TypeFace = Typeface.Purista;

        private FillFlowContainer flow;

        public TestSceneFont()
        {
            Children = new Drawable[]
            {
                new ScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        flow = new FillFlowContainer
                        {
                            Anchor = Anchor.TopLeft,
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Direction = FillDirection.Vertical,
                        }
                    }
                }
            };

            #region Steps
            AddStep("Purista", () =>
            {
                TypeFace = Typeface.Purista;
                Refresh();
            });

            AddStep("Regular", () =>
            {
                Weight = FontWeight.Regular;
                Refresh();
            });

            AddStep("Medium", () =>
            {
                Weight = FontWeight.Medium;
                Refresh();
            });
            #endregion

            Refresh();
        }

        private void Refresh()
        {
            flow.Clear();

            System.Console.WriteLine(TypeFace);
            System.Console.WriteLine(Weight);

            flow.Add(new SpriteText
            {
                Text = @"the quick red fox jumps over the lazy brown dog",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight)
            });
            flow.Add(new SpriteText
            {
                Text = @"THE QUICK RED FOX JUMPS OVER THE LAZY BROWN DOG",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight)
            });
            flow.Add(new SpriteText
            {
                Text = @"0123456789!@#$%^&*()_-+-[]{}.,<>;'\",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight)
            });
        }
    }
}
