using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;
using osuTK;
using Rhythmic.Graphics.UserInterface;
using System;
using System.Collections.Generic;

namespace Rhythmic.Test.Visual.Graphics
{
    public class TestSceneOutlinedSprite : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(OutlinedSprite)
        };

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Add(new OutlinedSprite
            {
                Size = new Vector2(500),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                BlurSigma = 10,
                Thickness = 20,
                Texture = store.Get("https://i.pinimg.com/originals/47/a3/49/47a3492df66e172b6e68af79d3c8b7e2.jpg")
            });
        }
    }
}
