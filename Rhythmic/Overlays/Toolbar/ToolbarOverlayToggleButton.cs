using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Toolbar
{
    public class ToolbarOverlayToggleButton : ToolbarButton
    {
        private readonly Box stateBackground;

        private OverlayContainer stateContainer;

        public OverlayContainer StateContainer
        {
            get => stateContainer;
            set
            {
                stateContainer = value;

                if (stateContainer != null)
                {
                    Action = stateContainer.ToggleVisibility;
                    stateContainer.StateChanged += stateChanged;
                }
            }
        }

        public ToolbarOverlayToggleButton()
        {
            Add(stateBackground = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = RhythmicColors.Gray(150).Opacity(180),
                Blending = BlendingMode.Additive,
                Depth = 2,
                Alpha = 0,
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (stateContainer != null)
                stateContainer.StateChanged -= stateChanged;
        }

        private void stateChanged(Visibility state)
        {
            switch (state)
            {
                case Visibility.Hidden:
                    stateBackground.FadeOut(200);
                    break;

                case Visibility.Visible:
                    stateBackground.FadeIn(200);
                    break;
            }
        }
    }
}
