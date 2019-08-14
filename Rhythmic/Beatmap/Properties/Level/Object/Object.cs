using osu.Framework.Graphics;
using Rhythmic.Beatmap.Properties.Level.Keyframe;
using System.Collections.Generic;
using System.Linq;

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

        public BlendingParameters BlendingParameters { get; set; }

        public bool Autokill { get; set; } = true;

        public bool Helper { get; set; } = false;

        public bool Empty { get; set; } = false;

        public bool Masking { get; set; } = false;

        public bool IsPartOfStoryBoard { get; set; } = false;

        public float TotalTime
        {
            get
            {
                float time = 0f;

                if (ColourKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in ColourKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (BorderColourKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in BorderColourKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (FillKeyframes?.Any() == true)
                    foreach (Keyframe<double> t in FillKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (InnerRadiusKeyframes?.Any() == true)
                    foreach (Keyframe<float> t in InnerRadiusKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (ShearKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in ShearKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (MoveKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in MoveKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (RotationKeyframes?.Any() == true)
                    foreach (Keyframe<double> t in RotationKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (ScaleKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in ScaleKeyframes)
                    {
                        time += (float)t.Time;
                    }

                if (BorderThicknessKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in BorderThicknessKeyframes)
                    {
                        time += (float)t.Time;
                    }

                return time;
            }
        }

        public float AbsoluteTotalTime
        {
            get
            {
                float time = 0f;
                float timeTilFinish = 0f;

                if (ColourKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in ColourKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (BorderColourKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in BorderColourKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (FillKeyframes?.Any() == true)
                    foreach (Keyframe<double> t in FillKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (InnerRadiusKeyframes?.Any() == true)
                    foreach (Keyframe<float> t in InnerRadiusKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (ShearKeyframes?.Any() == true)
                    foreach (Keyframe<float[]> t in ShearKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (MoveKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in MoveKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (RotationKeyframes?.Any() == true)
                    foreach (Keyframe<double> t in RotationKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (ScaleKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in ScaleKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                if (BorderThicknessKeyframes?.Any() == true)
                    foreach (Keyframe<double[]> t in BorderThicknessKeyframes)
                    {
                        if ((float)t.Time > time)
                            time = (float)t.Time;

                        if ((float)t.TimeUntilFinish > timeTilFinish)
                            timeTilFinish = (float)t.TimeUntilFinish;
                    }

                System.Console.WriteLine("Absolute Time: " + (time + timeTilFinish).ToString());

                return time + timeTilFinish;
            }
        }

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
