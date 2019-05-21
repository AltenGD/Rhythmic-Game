using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Screens.MainMenu.Components;
using System.IO;
using System.Text;
using static System.Environment;
using osu.Framework.Graphics.Containers;
using Rhythmic.Screens.Select;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Other;
using osuTK;
using Rhythmic.Screens.Edit;
using Rhythmic.Graphics.UserInterface;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using Rhythmic.Overlays.Toolbar;

namespace Rhythmic.Screens.Select
{
    public class InfoScreen : RhythmicScreen
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        public override bool DisableBeatmapOnEnter => true;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new Container
            {
                Size = new Vector2(150),
                Margin = new MarginPadding
                {
                    Top = Toolbar.HEIGHT + 150,
                    Left = 100
                },
                Children = new Drawable[]
                {
                    new ClickableContainer
                    {
                        Origin = Anchor.BottomLeft,
                        Masking = true,
                        BorderColour = Color4.White,
                        BorderThickness = 5,
                        Size = new Vector2(150),
                        Rotation = 45,
                        Action = () => this.Push(new Play.Play()),
                        Children = new Drawable[]
                        {
                            new Sprite
                            {
                                Rotation = -45,
                                Texture = collection.CurrentBeatmap.Value.Logo,
                                Position = new Vector2(-75, 75),
                                Anchor = Anchor.BottomRight,
                                Origin = Anchor.BottomLeft,
                                Size = new Vector2(215), // Strange precision
                            }
                        }
                    }
                }
            });
        }
    }
}
