using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace Rhythmic.Screens.Backgrounds
{
    public class BackgroundScreenWhite : BackgroundScreen
    {
        public BackgroundScreenWhite()
        {
            InternalChild = new Box
            {
                Colour = Color4.White,
                RelativeSizeAxes = Axes.Both,
            };
        }

        public override void OnEntering(IScreen last)
        {
            Show();
        }
    }
}
