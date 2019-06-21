using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Colors;
using System.Linq;
using osu.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osuTK.Input;
using Rhythmic.Graphics.UserInterface;

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
