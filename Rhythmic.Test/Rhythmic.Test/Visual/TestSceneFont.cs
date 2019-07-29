using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Test.Visual
{
    public class TestSceneFont : TestScene
    {
        private FontWeight Weight = FontWeight.Regular;
        private Typeface TypeFace = Typeface.Purista;
        private bool Italics = false;

        private readonly FillFlowContainer flow;

        public TestSceneFont()
        {
            Children = new Drawable[]
            {
                new RhythmicScrollContainer
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

            AddStep("Digitall", () =>
            {
                TypeFace = Typeface.Digitall;
                Refresh();
            });

            AddStep("Audiowide", () =>
            {
                TypeFace = Typeface.Audiowide;
                Refresh();
            });

            AddStep("Bold", () =>
            {
                Weight = FontWeight.Bold;
                Refresh();
            });

            AddStep("SemiBold", () =>
            {
                Weight = FontWeight.SemiBold;
                Refresh();
            });

            AddStep("Regular", () =>
            {
                Weight = FontWeight.Regular;
                Refresh();
            });

            AddStep("Light", () =>
            {
                Weight = FontWeight.Light;
                Refresh();
            });

            AddStep("Thin", () =>
            {
                Weight = FontWeight.Thin;
                Refresh();
            });

            AddStep("Italics", () =>
            {
                Italics = !Italics;
                Refresh();
            });
            #endregion

            Refresh();
        }

        private void Refresh()
        {
            flow.Clear();

            flow.Add(new SpriteText
            {
                Text = @"the quick red fox jumps over the lazy brown dog",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight, Italics)
            });
            flow.Add(new SpriteText
            {
                Text = @"THE QUICK RED FOX JUMPS OVER THE LAZY BROWN DOG",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight, Italics)
            });
            flow.Add(new SpriteText
            {
                Text = @"0123456789!@#$%^&*()_-+-[]{}.,<>;'\",
                Font = RhythmicFont.GetFont(TypeFace, 60, Weight, Italics)
            });
        }
    }
}
