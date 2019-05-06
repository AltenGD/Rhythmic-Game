using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using Rhythmic.Graphics;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Test.Visual
{
    public class TestCaseFont : TestCase
    {
        private RhythmicFont.FontWeight Weight = RhythmicFont.FontWeight.Regular;
        private RhythmicFont.Typeface TypeFace = RhythmicFont.Typeface.Purista;

        private FillFlowContainer flow;

        public TestCaseFont()
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
                TypeFace = RhythmicFont.Typeface.Purista;
                Refresh();
            });

            AddStep("Neogrey", () =>
            {
                TypeFace = RhythmicFont.Typeface.Neogrey;
                Refresh();
            });

            AddStep("AudioWide", () =>
            {
                TypeFace = RhythmicFont.Typeface.Audiowide;
                Refresh();
            });

            AddStep("Regular", () =>
            {
                Weight = RhythmicFont.FontWeight.Regular;
                Refresh();
            });

            AddStep("Medium", () =>
            {
                Weight = RhythmicFont.FontWeight.Medium;
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
