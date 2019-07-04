using System;
using osu.Framework.Graphics.UserInterface;

namespace Rhythmic.Graphics.UserInterface
{
    public class RhythmicMenuItem : MenuItem
    {
        public readonly MenuItemType Type;

        public RhythmicMenuItem(string text, MenuItemType type = MenuItemType.Standard)
            : base(text)
        {
            Type = type;
        }

        public RhythmicMenuItem(string text, MenuItemType type, Action action)
            : base(text, action)
        {
            Type = type;
        }
    }
}