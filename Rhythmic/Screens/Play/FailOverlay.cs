using osu.Framework.Allocation;
using osuTK.Graphics;

namespace Rhythmic.Screens.Play
{
    public class FailOverlay : GameplayMenuOverlay
    {
        public override string Header => "failed";
        public override string Description => "you're dead, try again?";

        [BackgroundDependencyLoader]
        private void load()
        {
            //AddButton("Retry", RhythmicColors.OrangeDark, () => OnRetry?.Invoke());
            AddButton("Quit", new Color4(170, 27, 39, 255), () => OnQuit?.Invoke());
        }
    }
}
