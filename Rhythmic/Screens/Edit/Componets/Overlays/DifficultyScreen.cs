using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;

namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class DifficultyScreen : BeatmapMetadataScreen
    {
        public DifficultyScreen()
        {
            AddInternal(new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Difficulty Screen"
            });
        }
    }
}
