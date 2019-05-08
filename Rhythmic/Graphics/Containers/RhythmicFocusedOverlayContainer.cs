using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using Rhythmic.Overlays;

namespace Rhythmic.Graphics.Containers
{
    public class RhythmicFocusedOverlayContainer : FocusedOverlayContainer
    {
        protected override bool BlockPositionalInput => base.BlockPositionalInput;

        /// <summary>Temporary to allow for overlays in the main screen content to not dim theirselves.</summary>
        protected virtual bool DimMainContent => true;

        [Resolved(CanBeNull = true)]
        private RhythmicGame game { get; set; }

        protected readonly Bindable<OverlayActivation> OverlayActivationMode = new Bindable<OverlayActivation>(OverlayActivation.All);

        [BackgroundDependencyLoader]
        private void load()
        {
            StateChanged += onStateChanged;
        }

        /// <summary> Whether mouse input should be blocked screen-wide while this overlay is visible.
        /// Performing mouse actions outside of the valid extents will hide the overlay.</summary>
        public virtual bool BlockScreenWideMouse => BlockPositionalInput;

        // receive input outside our bounds so we can trigger a close event on ourselves.
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => BlockScreenWideMouse || base.ReceivePositionalInputAt(screenSpacePos);

        protected override bool OnClick(ClickEvent e)
        {
            if (!base.ReceivePositionalInputAt(e.ScreenSpaceMousePosition))
            {
                State = Visibility.Hidden;
                return true;
            }

            return base.OnClick(e);
        }

        private void onStateChanged(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Visible:
                    if (OverlayActivationMode.Value != OverlayActivation.Disabled)
                    {
                        if (BlockScreenWideMouse && DimMainContent) game?.AddBlockingOverlay(this);
                    }
                    else
                        State = Visibility.Hidden;

                    break;

                case Visibility.Hidden:
                    if (BlockScreenWideMouse) game?.RemoveBlockingOverlay(this);
                    break;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            game?.RemoveBlockingOverlay(this);
        }
    }
}
