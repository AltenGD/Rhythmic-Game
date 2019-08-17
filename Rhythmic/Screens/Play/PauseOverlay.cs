using System;
using System.Linq;
using osu.Framework.Allocation;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Screens.Play
{
    public class PauseOverlay : GameplayMenuOverlay
    {
        public Action OnResume;

        public override string Header => "paused";
        public override string Description => "you're not going to do what i think you're going to do, are ya?";

        protected override Action BackAction => () => InternalButtons.Children.First().Click();

        [BackgroundDependencyLoader]
        private void load()
        {
            AddButton("Continue", RhythmicColors.FromHex("88b300"), () => OnResume?.Invoke());
            AddButton("Retry", RhythmicColors.FromHex(@"eeaa00"), () => OnRetry?.Invoke());
            AddButton("Quit", new Color4(170, 27, 39, 255), () => OnQuit?.Invoke());
        }
    }
}
