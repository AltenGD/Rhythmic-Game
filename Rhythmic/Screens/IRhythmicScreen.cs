using osu.Framework.Bindables;
using osu.Framework.Screens;
using Rhythmic.Overlays;

namespace Rhythmic.Screens
{
    interface IRhythmicScreen
    {
        /// <summary>The amount of parallax to be applied while this screen is displayed.</summary>
        float BackgroundParallaxAmount { get; }

        /// <summary>Whether all overlays should be hidden when this screen is entered or resumed.</summary>
        bool HideOverlaysOnEnter { get; }

        /// <summary>Whether the current beatmap should be disabled or not.</summary>
        bool DisableBeatmapOnEnter { get; }

        /// <summary>Whether overlays should be able to be opened once this screen is entered or resumed.</summary>
        OverlayActivation InitialOverlayActivationMode { get; }
    }
}
