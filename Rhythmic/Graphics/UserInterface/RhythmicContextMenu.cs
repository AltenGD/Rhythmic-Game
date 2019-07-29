using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Effects;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.UserInterface
{
    public class RhythmicContextMenu : RhythmicMenu
    {
        private const int fade_duration = 250;

        public RhythmicContextMenu()
            : base(Direction.Vertical)
        {
            MaskingContainer.CornerRadius = 5;
            MaskingContainer.EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Colour = Color4.Black.Opacity(0.1f),
                Radius = 4,
            };

            ItemsContainer.Padding = new MarginPadding { Vertical = DrawableRhythmicMenuItem.MARGIN_VERTICAL };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            BackgroundColour = RhythmicColors.FromHex("223034");
        }

        protected override void AnimateOpen() => this.FadeIn(fade_duration, Easing.OutQuint);
        protected override void AnimateClose() => this.FadeOut(fade_duration, Easing.OutQuint);
    }
}
