using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class SongScreen : RhythmicScreen
    {
        public SongScreen()
        {
            AddInternal(new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Song Screen"
            });
        }
    }
}
