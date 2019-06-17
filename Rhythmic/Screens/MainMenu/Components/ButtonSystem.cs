using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;
using osuTK;
using System;
using Rhythmic.Graphics.Colors;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using Rhythmic.Database;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;

namespace Rhythmic.Screens.MainMenu.Components
{
    public class ButtonSystem : FillFlowContainer
    {
        public Action OnPlay;
        public Action OnEditor;

        private BufferedContainer screen;

        public ButtonSystem(BufferedContainer Screen)
        {
            screen = Screen;
        }

        [BackgroundDependencyLoader]
        private void load(RhythmicStore store)
        {
            Direction = FillDirection.Vertical;
            Spacing = new Vector2(0, 10);
            AutoSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    CornerRadius = 10,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = store.SecondaryColour.Value.Opacity(0.2f)
                        },
                        new BufferedContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            BackgroundColour = Color4.Black,
                            BlurSigma = new Vector2(15),
                            Child = screen.CreateView().With(d =>
                            {
                                d.RelativeSizeAxes = Axes.Both;
                                d.SynchronisedDrawQuad = true;
                            })
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 20),
                            AutoSizeAxes = Axes.Y,
                            Width = 300,
                            Padding = new MarginPadding
                            {
                                Vertical = 20
                            },
                            Children = new Drawable[]
                            {
                                new MenuButton
                                {
                                    Action = () => OnPlay?.Invoke(),
                                    Icon = FontAwesome.Solid.Play,
                                    Text = "Play",
                                },
                                new MenuButton
                                {
                                    Action = () => OnEditor?.Invoke(),
                                    Icon = FontAwesome.Solid.Wrench,
                                    Text = "Editor"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
