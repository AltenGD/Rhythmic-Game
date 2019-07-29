using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class Header : Container
    {
        private SpriteText tabText;

        public RhythmicTabControl TabControl;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRange(new Drawable[]
            {
                new FillFlowContainer
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Spacing = new Vector2(15, 0),
                    Children = new Drawable[]
                    {
                        new HexagonalIcon
                        {
                            Icon = FontAwesome.Solid.Pen,
                            Size = new Vector2(50),
                            Resolution = HexagonResolution.TenTimes
                        },
                        new SpriteText
                        {
                            Font = RhythmicFont.Default,
                            Text = "Beatmap Metadata"
                        },
                        tabText = new SpriteText
                        {
                            Font = RhythmicFont.Default,
                            Colour = RhythmicColors.Blue,
                        }
                    }
                },
                TabControl = new RhythmicTabControl
                {
                    AccentColour = RhythmicColors.Blue,
                    RelativeSizeAxes = Axes.X,
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Height = 30,
                }
            });

            TabControl.AddItem("General");
            TabControl.AddItem("Song");
            TabControl.AddItem("Difficulty");

            TabControl.Current.ValueChanged += val =>
            {
                tabText.Text = val.NewValue;
            };
        }
    }
}
