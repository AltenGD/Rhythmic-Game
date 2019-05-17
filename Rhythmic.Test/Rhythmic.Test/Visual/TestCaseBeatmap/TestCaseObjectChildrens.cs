using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Allocation;
using Rhythmic.Beatmap.Properties.Drawables;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Beatmap.Properties.Level.Object;
using System.Collections.Generic;
using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Level.Keyframe;

namespace Rhythmic.Test.Visual.TestCaseBeatmap
{
    public class TestCaseObjectChildrens : TestCase
    {
        private BeatmapMeta TestLevel;
        private BeatmapAPI API;

        [BackgroundDependencyLoader]
        private void load()
        {
            API = new BeatmapAPI();

            TestLevel = API.CreateNewLevel(new BeatmapMeta
            {
                Metadata = new BeatmapMetadata
                {
                    Level = new LevelMetadata
                    {
                        CreatorID = 0,
                        CreatorName = "Alten",
                        LevelName = "NULCTRL"
                    },
                    Song = new SongMetadata
                    {
                        Author = "SilentRoom",
                        Name = "NULCTRL"
                    }
                },
                Level = new LevelMeta
                {
                    BackgroundColour = new float[] { 20, 20, 20 }
                }
            });

            API.AddObject(new Object
            {
                AutoSizeAxes = Axes.Both,
                RelativeSizeAxes = Axes.None,
                Size = new float[] { 1f, 1f },
                Position = new float[] { 0, -1900 },
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Autokill = false,
                Colour = new float[] { 255, 255, 255, 255 },
                Masking = false,
                MoveKeyframes = new List<Keyframe<double[]>>
                {
                    new Keyframe<double[]>
                    {
                        EaseType = Easing.OutExpo,
                        Value = new double[] { 0, 0 },
                        Time = 0,
                        TimeUntilFinish = 6000
                    }
                },
                ColourKeyframes = new List<Keyframe<float[]>>(),
                RotationKeyframes = new List<Keyframe<double>>(),
                ScaleKeyframes = new List<Keyframe<double[]>>(),
                BorderColourKeyframes = new List<Keyframe<float[]>>(),
                BorderThicknessKeyframes = new List<Keyframe<double[]>>(),
                BorderColour = new float[] { 255, 255, 255, 255 },
                BorderThickness = 0f,
                Name = "Test Container",
                Rotation = 0,
                Shape = Shape.Container,
                Time = 0,
                Childrens = new List<Object>()
                {
                    new Object
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Colour = new float[] { 255, 255, 255, 255 },
                        Size = new float[] { 190, 190 },
                        Position = new float[] { 0, 0 },
                        AutoSizeAxes = Axes.Both,
                        RelativeSizeAxes = Axes.None,
                        Autokill = false,
                        Masking = false,
                        MoveKeyframes = new List<Keyframe<double[]>>(),
                        ColourKeyframes = new List<Keyframe<float[]>>(),
                        RotationKeyframes = new List<Keyframe<double>>(),
                        ScaleKeyframes = new List<Keyframe<double[]>>(),
                        Name = "Test Square",
                        Rotation = 0,
                        Shape = Shape.Square,
                        Time = 0,
                    },
                    new Object
                    {
                        AutoSizeAxes = Axes.None,
                        RelativeSizeAxes = Axes.None,
                        Size = new float[] { 125f, 125f },
                        Position = new float[] { 0, 0 },
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Autokill = false,
                        Colour = new float[] { 255, 255, 0, 255 },
                        Masking = true,
                        MoveKeyframes = new List<Keyframe<double[]>>(),
                        ColourKeyframes = new List<Keyframe<float[]>>(),
                        RotationKeyframes = new List<Keyframe<double>>(),
                        ScaleKeyframes = new List<Keyframe<double[]>>(),
                        BorderColourKeyframes = new List<Keyframe<float[]>>(),
                        BorderThicknessKeyframes = new List<Keyframe<double[]>>(),
                        BorderColour = new float[] { 255, 255, 255, 255 },
                        BorderThickness = 0f,
                        Name = "Test Circle",
                        Rotation = 0,
                        Shape = Shape.Circle,
                        Time = 0,
                        Childrens = new List<Object>()
                    },
                    new Object
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Colour = new float[] { 255, 0, 255, 150 },
                        Size = new float[] { 125, 125 },
                        Position = new float[] { 0, 0 },
                        AutoSizeAxes = Axes.Both,
                        RelativeSizeAxes = Axes.None,
                        Autokill = false,
                        Masking = false,
                        MoveKeyframes = new List<Keyframe<double[]>>(),
                        ColourKeyframes = new List<Keyframe<float[]>>(),
                        RotationKeyframes = new List<Keyframe<double>>(),
                        ScaleKeyframes = new List<Keyframe<double[]>>(),
                        Name = "Test Triangle",
                        Rotation = 0,
                        Shape = Shape.Triangle,
                        Time = 0,
                    },
                    new Object
                    {
                        Origin = Anchor.Centre,
                        Anchor = Anchor.Centre,
                        Colour = new float[] { 0, 255, 255, 150 },
                        Size = new float[] { 70, 70 },
                        Position = new float[] { 0, 0 },
                        AutoSizeAxes = Axes.Both,
                        RelativeSizeAxes = Axes.None,
                        Autokill = false,
                        Masking = false,
                        MoveKeyframes = new List<Keyframe<double[]>>(),
                        ColourKeyframes = new List<Keyframe<float[]>>(),
                        RotationKeyframes = new List<Keyframe<double>>(),
                        ScaleKeyframes = new List<Keyframe<double[]>>(),
                        Name = "Test EquilateralTriangle",
                        Rotation = 0,
                        Shape = Shape.EquilateralTriangle,
                        Time = 0,
                    },
                }
            }, TestLevel);

            foreach (var obj in TestLevel.Level.Level)
            {
                Scheduler.AddDelayed(() =>
                {
                    Add(new DrawableBeatmapObject(obj));
                }, obj.Time);
            }
        }
    }
}
