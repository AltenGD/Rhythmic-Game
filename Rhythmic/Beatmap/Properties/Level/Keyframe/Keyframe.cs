using osu.Framework.Graphics;

namespace Rhythmic.Beatmap.Properties.Level.Keyframe
{
    public class Keyframe<T>
    {
        public double Time { get; set; }

        public double TimeUntilFinish { get; set; }

        public T Value { get; set; }

        public Easing EaseType { get; set; }
    }
}