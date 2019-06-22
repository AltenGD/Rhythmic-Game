using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.MathUtils;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.UserInterface;
using Rhythmic.Other;
using Rhythmic.Overlays;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Visualizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Environment;

namespace Rhythmic.Screens
{
    public class VisualizerScreen : RhythmicScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        private OutlinedSprite beatmapSprite;

        [BackgroundDependencyLoader]
        private void load(BeatmapCollection collection)
        {
            collection.CurrentBeatmap.ValueChanged += OnBeatmapChanged;

            AddRangeInternal(new Drawable[]
            {
                new SpaceParticlesContainer(),
                new CircularVisualizer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    ValueMultiplier = 800,
                    DegreeValue = 180,
                    BarsAmount = 100,
                    CircleSize = 348,
                    BarWidth = 2,
                },
                new CircularVisualizer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    ValueMultiplier = 800,
                    DegreeValue = 180,
                    BarsAmount = 100,
                    CircleSize = 348,
                    BarWidth = 2,
                    Rotation = 180,
                },
                new CircularContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(350),
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.Black.Opacity(0.18f),
                        Offset = new Vector2(0, 2),
                        Radius = 6,
                    },
                    Child = beatmapSprite = new OutlinedSprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        FillMode = FillMode.Fill,
                        BlurSigma = 10
                    }
                },
            });

            beatmapSprite.Texture = collection.CurrentBeatmap.Value.Logo ?? collection.CurrentBeatmap.Value.Background;
        }

        private void OnBeatmapChanged(ValueChangedEvent<BeatmapMeta> obj)
        {
            beatmapSprite.Texture = obj.NewValue.Logo ?? obj.NewValue.Background;
        }
    }
}
