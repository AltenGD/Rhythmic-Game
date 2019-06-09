using System.ComponentModel;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Test.Visual.UserInterface
{
    public class TestSceneTabControl : TestScene
    {
        public TestSceneTabControl()
        {
            SpriteText text;
            RhythmicTabControl tabControl;

            Add(tabControl = new RhythmicTabControl
            {
                AccentColour = RhythmicColors.Blue,
                Margin = new MarginPadding(4),
                Position = new Vector2(0, 20),
                RelativeSizeAxes = Axes.X,
                Size = new Vector2(1, 10),
                AutoSort = true,
            });

            Add(text = new SpriteText
            {
                Text = "None",
                Margin = new MarginPadding(4),
                Position = new Vector2(0, 55)
            });

            tabControl.AddItem("Test1");
            tabControl.AddItem("Test2");
            tabControl.AddItem("Test3");
            tabControl.AddItem("Test4");
            tabControl.AddItem("yes");
            tabControl.AddItem("no");
            tabControl.AddItem("maybe");

            tabControl.Current.ValueChanged += value =>
            {
                text.Text = "Currently Selected: " + value.NewValue.ToString();
            };
        }
    }
}
