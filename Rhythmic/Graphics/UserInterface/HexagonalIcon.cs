using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;
using Rhythmic.Graphics.Containers;

namespace Rhythmic.Graphics.UserInterface
{
    public class HexagonalIcon : Container
    {
        public IconUsage Icon { get; set; }

        public Color4 HexColor = Color4.White;

        public Color4 IconColor = Color4.White;

        public HexagonResolution Resolution = HexagonResolution.OneTimes;

        public bool Sharp;

        private Sprite hexagon;

        [Resolved]
        private TextureStore store { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                hexagon = new Sprite
                {
                    Size = DrawSize,
                    Colour = HexColor,
                },
                new SpriteIcon
                {
                    Size = new Vector2(DrawSize.X * 0.3f, DrawSize.Y * 0.3f),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = IconColor,
                    Icon = Icon,
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            switch (Resolution)
            {
                case HexagonResolution.OneTimes:
                    hexagon.Texture = Sharp ? store.Get(@"Objects/Hexagon/HexagonSharpOnex") : store.Get(@"Objects/Hexagon/HexagonOnex");
                    break;
                case HexagonResolution.TwoTimes:
                    hexagon.Texture = Sharp ? store.Get(@"Objects/Hexagon/HexagonSharpTwox") : store.Get(@"Objects/Hexagon/HexagonTwox");
                    break;
                case HexagonResolution.TenTimes:
                    hexagon.Texture = Sharp ? store.Get(@"Objects/Hexagon/HexagonSharpTenx") : store.Get(@"Objects/Hexagon/HexagonTenx");
                    break;
            }
        }
    }

    public enum HexagonResolution
    {
        OneTimes,
        TwoTimes,
        TenTimes
    }
}
