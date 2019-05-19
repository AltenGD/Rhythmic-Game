using Rhythmic.Beatmap.Properties.Level.Keyframe;
using osu.Framework.Graphics;
using System.Collections.Generic;

namespace Rhythmic.Beatmap.Properties.Level.Object
{
    public class Object
    {
        public string Name { get; set; } = "Object";

        public double Time { get; set; } = 0;

        public Shape Shape { get; set; } = Shape.Square;

        public string ImageUrl { get; set; } = "";

        public float[] Colour { get; set; } = new float[] { 255, 255, 255, 255 };

        public float[] BorderColour { get; set; } = new float[] { 255, 255, 255, 255 };

        public float[] Position { get; set; } = new float[] { 0, 0 };

        public float[] Size { get; set; } = new float[] { 25, 25 };

        public float[] Shear { get; set; } = new float[] { 0, 0 };

        public double Fill { get; set; } = 1;

        public float InnerRadius { get; set; } = 1;

        public float Rotation { get; set; } = 0f;

        public float Depth { get; set; } = 0;

        public float BorderThickness { get; set; } = 0f;

        public Anchor Anchor { get; set; } = Anchor.TopLeft;

        public Anchor Origin { get; set; } = Anchor.TopLeft;

        public Axes RelativeSizeAxes { get; set; } = Axes.None;

        public Axes AutoSizeAxes { get; set; } = Axes.None;

        public bool Autokill { get; set; } = true;

        public bool Helper { get; set; } = false;

        public bool Empty { get; set; } = false;

        public bool Masking { get; set; } = false;

        public bool IsPartOfStoryBoard { get; set; } = false;

        public List<Object> Childrens { get; set; } = new List<Object>();

        public List<Keyframe<float[]>> ColourKeyframes { get; set; } = new List<Keyframe<float[]>>();

        public List<Keyframe<float[]>> BorderColourKeyframes { get; set; } = new List<Keyframe<float[]>>();

        public List<Keyframe<double>> FillKeyframes { get; set; } = new List<Keyframe<double>>();

        public List<Keyframe<float>> InnerRadiusKeyframes { get; set; } = new List<Keyframe<float>>();

        public List<Keyframe<float[]>> ShearKeyframes { get; set; } = new List<Keyframe<float[]>>();

        public List<Keyframe<double[]>> MoveKeyframes { get; set; } = new List<Keyframe<double[]>>();

        public List<Keyframe<double[]>> ScaleKeyframes { get; set; } = new List<Keyframe<double[]>>();

        public List<Keyframe<double[]>> BorderThicknessKeyframes { get; set; } = new List<Keyframe<double[]>>();

        public List<Keyframe<double>> RotationKeyframes { get; set; } = new List<Keyframe<double>>();
    }
}
