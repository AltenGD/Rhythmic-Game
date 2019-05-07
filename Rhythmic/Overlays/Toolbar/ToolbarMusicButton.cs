using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;

namespace Rhythmic.Overlays.Toolbar
{
    public class ToolbarMusicButton : ToolbarOverlayToggleButton
    {
        public ToolbarMusicButton()
        {
            Icon = FontAwesome.Solid.Music;
        }

        [BackgroundDependencyLoader(true)]
        private void load()
        {
            
        }
    }
}
