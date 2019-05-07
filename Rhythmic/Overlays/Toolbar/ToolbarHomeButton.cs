using osu.Framework.Graphics.Sprites;

namespace Rhythmic.Overlays.Toolbar
{
    public class ToolbarHomeButton : ToolbarButton
    {
        public ToolbarHomeButton()
        {
            Icon = FontAwesome.Solid.Home;
            TooltipMain = "Home";
            TooltipSub = "Return to the main menu";
        }
    }
}
