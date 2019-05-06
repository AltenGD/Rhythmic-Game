using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace Rhythmic.Screens
{
    interface IRhythmicScreen
    {
        /// <summary>
        /// The amount of parallax to be applied while this screen is displayed.
        /// </summary>
        float BackgroundParallaxAmount { get; }
    }
}
