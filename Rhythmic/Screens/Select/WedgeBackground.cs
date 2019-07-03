using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

namespace Rhythmic.Screens.Select
{
    public class WedgeBackground : Container
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Children = new[]
            {
                new Sprite
                {
                    Texture = store.Get("Objects/CircularSubstraction"),
                    Colour = Color4.Black,
                    Alpha = 0.25f
                }
            };
        }
    }
}
