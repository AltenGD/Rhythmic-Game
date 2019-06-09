using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Test.Visual.UserInterface
{
    public class TestSceneHexagonalIcon : TestScene
    {
        public TestSceneHexagonalIcon()
        {
            Children = new Drawable[]
            {
                new HexagonalIcon
                {
                    Size = new Vector2(150),
                    Icon = FontAwesome.Solid.Pen,
                    Resolution = HexagonResolution.TenTimes
                }
            };

            AddToggleStep("Sharp", val =>
            {
                Clear();

                Add(new HexagonalIcon
                {
                    Size = new Vector2(150),
                    Icon = FontAwesome.Solid.Pen,
                    Resolution = HexagonResolution.TenTimes,
                    Sharp = val
                });
            });
        }
    }
}
