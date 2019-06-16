using osu.Framework.Bindables;
using osuTK.Graphics;

namespace Rhythmic.Database
{
    /// <summary>Includes stuff such as Primary Colors, Beatmaps, Scores, etc.</summary>
    public class RhythmicStore
    {
        public Bindable<Color4> PrimaryColour = new Bindable<Color4>();

        public Bindable<Color4> SecondaryColour = new Bindable<Color4>();
    }
}
