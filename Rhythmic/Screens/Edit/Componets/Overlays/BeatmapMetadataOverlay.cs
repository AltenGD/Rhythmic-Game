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
using osu.Framework.Extensions.Color4Extensions;

namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class BeatmapMetadataOverlay : RhythmicFocusedOverlayContainer
    {
        private readonly RhythmicScreenStack screenStack;

        private Header header;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new Container
            {
                Masking = true,
                CornerRadius = 10,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black.Opacity(0.4f)
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Size = new Vector2(1f, 100),
                        Colour = Color4.Black.Opacity(0.2f)
                    },
                    header = new Header
                    {
                        Size = new Vector2(1f, 100),
                        RelativeSizeAxes = Axes.X,
                        Depth = -1,
                        Padding = new MarginPadding
                        {
                            Horizontal = 10,
                            Top = 10
                        },
                    }
                }
            });
        }
    }
}
