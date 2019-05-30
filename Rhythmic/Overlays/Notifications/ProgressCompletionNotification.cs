using osu.Framework.Allocation;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Notifications
{
    public class ProgressCompletionNotification : SimpleNotification
    {
        public ProgressCompletionNotification()
        {
            Icon = FontAwesome.Solid.Check;
            IconBackgound.Colour = RhythmicColors.GreenDark;
        }
    }
}
