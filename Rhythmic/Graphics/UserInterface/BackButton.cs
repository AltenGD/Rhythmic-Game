using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.UserInterface
{
    public class BackButton : Button 
    {
        public BackButton()
        {
            Text = @"back";
            BackgroundColour = RhythmicColors.HotPinkeDark;
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;
        }
    }
}
