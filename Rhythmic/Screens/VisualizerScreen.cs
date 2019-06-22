using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.UserInterface;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Visualizers;

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
