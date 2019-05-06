using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using Rhythmic.Graphics.Containers;

namespace Rhythmic.Screens
{
    public class RhythmicScreenStack : ScreenStack
    {
        [Cached]
        private BackgroundScreenStack backgroundScreenStack;

        private ParallaxContainer parallaxContainer;

        protected float ParallaxAmount => parallaxContainer.ParallaxAmount;

        public RhythmicScreenStack()
        {
            initializeStack();
        }

        public RhythmicScreenStack(IScreen baseScreen)
            : base(baseScreen)
        {
            initializeStack();
        }

        private void initializeStack()
        {
            InternalChild = parallaxContainer = new ParallaxContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = backgroundScreenStack = new BackgroundScreenStack { RelativeSizeAxes = Axes.Both },
            };

            ScreenPushed += onScreenChange;
            ScreenExited += onScreenChange;
        }

        private void onScreenChange(IScreen prev, IScreen next)
        {
            parallaxContainer.ParallaxAmount = ParallaxContainer.DEFAULT_PARALLAX_AMOUNT * ((IRhythmicScreen)next)?.BackgroundParallaxAmount ?? 1.0f;
        }
    }
}
