using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.Sprites
{
    public class SpriteTextLink : SpriteText, IHasTooltip
    {
        private GameHost host;
        private readonly string url;

        public string TooltipText { get; set; }

        public SpriteTextLink(string url)
            => this.url = url;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            Colour = RhythmicColors.FromHex("5AB0FF");
            this.host = host;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (url != null)
                host.OpenUrlExternally(url);

            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.FadeColour(RhythmicColors.FromHex("CCE7FF"), 200, Easing.OutExpo);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.FadeColour(RhythmicColors.FromHex("5AB0FF"), 200, Easing.OutExpo);
            base.OnHoverLost(e);
        }
    }
}
