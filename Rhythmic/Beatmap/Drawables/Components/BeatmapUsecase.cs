using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Beatmap.Drawables.Components
{
    public class BeatmapUsecase : Container
    {
        private TextFlowContainer textContainer;

        private readonly BeatmapMeta beatmap;

        private readonly BufferedContainer background;

        public BeatmapUsecase(BeatmapMeta beatmap, BufferedContainer background)
        {
            this.beatmap = beatmap;
            this.background = background;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    BackgroundColour = Color4.Black,
                    BlurSigma = new Vector2(5),
                    Child = background.CreateView().With(d =>
                    {
                        d.RelativeSizeAxes = Axes.Both;
                        d.SynchronisedDrawQuad = true;
                    })
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White.Opacity(0.8f)
                },
                new Sprite
                {
                    Texture = store.Get("AcrylicNoise.png"),
                    Colour = Color4.Black.Opacity(0.05f),
                    Scale = new Vector2(2)
                },
                new Container
                {
                    Padding = new MarginPadding(5),
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        textContainer = new TextFlowContainer
                        {
                            Direction = FillDirection.Horizontal
                        }
                    }
                }
            };

            textContainer.AddText(new SpriteText
            {
                Text = "mapped by ",
                Colour = Color4.Black
            });

            textContainer.AddText(new SpriteTextLink(null)
            {
                Text = beatmap.Metadata.Level.CreatorName
            });
        }
    }
}
