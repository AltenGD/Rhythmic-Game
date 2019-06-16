using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.UserInterface
{
    public class BackButton : Button 
    {
        public BackButton()
        {
            Size = new Vector2(100, 50);
            Text = @"back";
            BackgroundColour = RhythmicColors.HotPinkDark;
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
        }
    }
}
