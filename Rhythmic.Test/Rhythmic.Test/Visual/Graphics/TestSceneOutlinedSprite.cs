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
                Size = new Vector2(200),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                BlurSigma = 10,
                Texture = store.Get("https://cdn.discordapp.com/avatars/184061887212814336/5213e87197c1ff3e94b549afc1ebe5bf.png?size=1024")
            });
        }
    }
}
