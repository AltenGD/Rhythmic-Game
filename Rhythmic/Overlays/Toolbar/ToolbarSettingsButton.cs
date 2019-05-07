using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace Rhythmic.Overlays.Toolbar
{
    public class ToolbarSettingsButton : ToolbarOverlayToggleButton
    {
        public ToolbarSettingsButton()
        {
            Icon = FontAwesome.Solid.Cog;
            TooltipMain = "Settings";
            TooltipSub = "Change your settings";
        }

        [BackgroundDependencyLoader(true)]
        private void load()
        {

        }
    }
}
