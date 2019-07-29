using Rhythmic.Overlays;

namespace Rhythmic.Screens
{
    internal interface IRhythmicScreen
    {
        /// <summary>The amount of parallax to be applied while this screen is displayed.</summary>
        float BackgroundParallaxAmount { get; }

        /// <summary>Whether all overlays should be hidden when this screen is entered or resumed.</summary>
        bool HideOverlaysOnEnter { get; }

        /// <summary>Whether the current beatmap should be disabled or not.</summary>
        bool DisableBeatmapOnEnter { get; }

        /// <summary>Whether a top-level component should be allowed to exit the current screen to, for example,
        /// complete an import. Note that this can be overridden by a user if they specifically request.</summary>
        bool AllowExternalScreenChange { get; }

        /// <summary>Whether overlays should be able to be opened once this screen is entered or resumed.</summary>
        OverlayActivation InitialOverlayActivationMode { get; }
    }
}
