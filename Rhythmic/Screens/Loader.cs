using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osu.Framework.Screens;
using Rhythmic.Other;
using Rhythmic.Screens.MainMenu;

namespace Rhythmic.Screens
{
    public class Loader : RhythmicScreen
    {
        public Loader()
        {
            ValidForResume = false;
        }

        protected override void LogoArriving(RhythmicLogo logo, bool resuming)
        {
            base.LogoArriving(logo, resuming);

            logo.RelativePositionAxes = Axes.None;
            logo.Origin = Anchor.BottomRight;
            logo.Anchor = Anchor.BottomRight;
            logo.Position = new Vector2(-40);
            logo.Scale = new Vector2(0.2f);

            logo.Delay(500).FadeInFromZero(1000, Easing.OutQuint);
        }

        protected override void LogoSuspending(RhythmicLogo logo)
        {
            base.LogoSuspending(logo);
            logo.FadeOut(logo.Alpha * 400);
        }

        private RhythmicScreen loadableScreen;

        protected virtual RhythmicScreen CreateLoadableScreen() => new Intro();

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            LoadComponentAsync(loadableScreen = CreateLoadableScreen());

            checkIfLoaded();
        }

        private void checkIfLoaded()
        {
            if (loadableScreen.LoadState != LoadState.Ready)
            {
                Schedule(checkIfLoaded);
                return;
            }

            this.Push(loadableScreen);
        }
    }
}
