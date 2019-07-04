using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Graphics.Cursor
{
    public class RhythmicContextMenuContainer : ContextMenuContainer
    {
        protected override Menu CreateMenu() => new RhythmicContextMenu();
    }
}
