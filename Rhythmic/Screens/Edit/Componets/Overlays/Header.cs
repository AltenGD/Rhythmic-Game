using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;
using System.Linq;
using osu.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osuTK.Input;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.UserInterface;
using Rhythmic.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;

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
