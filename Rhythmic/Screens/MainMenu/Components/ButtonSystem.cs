using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Screens.MainMenu.Components
{
    public class ButtonSystem : FillFlowContainer
    {
        public Action OnPlay;
        public Action OnEditor;

        public ButtonSystem()
        {
            Direction = FillDirection.Vertical;
            AutoSizeAxes = Axes.Both;
            Spacing = new Vector2(0, 10);

            Children = new Drawable[]
            {
                new RhythmicRectangleButton("Play", FontAwesome.Solid.Play, RhythmicColors.Green, RhythmicColors.GreenLight, () => OnPlay?.Invoke()),
                new RhythmicRectangleButton("Editor", FontAwesome.Solid.Wrench, RhythmicColors.Orange, RhythmicColors.OrangeLight, () => OnEditor?.Invoke()),
            };
        }
    }
}
