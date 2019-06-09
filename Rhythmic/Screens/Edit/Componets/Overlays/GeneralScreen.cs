using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;

namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class GeneralScreen : BeatmapMetadataScreen
    {
        public GeneralScreen()
        {
            AddInternal(new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "General Screen"
            });
        }
    }
}
