using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Screens;
using osuTK;
using Rhythmic.Beatmap;
using Rhythmic.Graphics.Containers;
using Rhythmic.Other;
using Rhythmic.Overlays;

namespace Rhythmic.Screens
{
    public abstract class RhythmicScreen : Screen, IRhythmicScreen, IKeyBindingHandler<GlobalAction>
    {
        /// <summary>A user-facing title for this screen. </summary>
        public virtual string Title => GetType().ShortDisplayName();

        public string Description => Title;

        public virtual float BackgroundParallaxAmount => 1;

        protected virtual bool AllowBackButton => true;

        public virtual bool HideOverlaysOnEnter => false;

        public virtual bool DisableBeatmapOnEnter => false;

        /// <summary>Whether overlays should be able to be opened once this screen is entered or resumed.</summary>
        public virtual OverlayActivation InitialOverlayActivationMode => OverlayActivation.All;

        protected BackgroundScreen Background => backgroundStack?.CurrentScreen as BackgroundScreen;

        private BackgroundScreen localBackground;

        [Resolved(canBeNull: true)]
        private BackgroundScreenStack backgroundStack { get; set; }

        [Resolved(canBeNull: true)]
        private RhythmicLogo logo { get; set; }

        [Resolved(canBeNull: true)]
        private BeatmapCollection collection { get; set; }

        protected RhythmicScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (DisableBeatmapOnEnter)
                collection.CurrentBeatmap.Disabled = true;
            else
                collection.CurrentBeatmap.Disabled = false;
        }

        public override void OnResuming(IScreen last)
        {
            applyArrivingDefaults(true);

            base.OnResuming(last);
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);
            onSuspendingLogo();
        }

        public override void OnEntering(IScreen last)
        {
            applyArrivingDefaults(false);

            backgroundStack?.Push(localBackground = CreateBackground());

            this.FadeIn(500, Easing.OutExpo);
            this.MoveTo(new Vector2(10, 0));
            this.MoveTo(new Vector2(0, 0), 500, Easing.OutExpo);

            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            if (ValidForResume && logo != null)
                onExitingLogo();

            if (base.OnExiting(next))
                return true;

            this.FadeOut(500, Easing.OutExpo);
            this.MoveTo(new Vector2(10, 0), 500, Easing.OutExpo);

            if (localBackground != null && backgroundStack?.CurrentScreen == localBackground)
                backgroundStack?.Exit();

            return false;
        }

        /// <summary>Fired when this screen was entered or resumed and the logo state is required to be adjusted.</summary>
        protected virtual void LogoArriving(RhythmicLogo logo, bool resuming)
        {
            ApplyLogoArrivingDefaults(logo);
        }

        private void applyArrivingDefaults(bool isResuming)
        {
            logo?.AppendAnimatingAction(() =>
            {
                if (this.IsCurrentScreen()) LogoArriving(logo, isResuming);
            }, true);
        }

        /// <summary>Applies default animations to an arriving logo.
        /// Todo: This should not exist.</summary>
        /// <param name="logo">The logo to apply animations to.</param>
        public static void ApplyLogoArrivingDefaults(RhythmicLogo logo)
        {
            logo.FadeOut(300, Easing.OutQuint);
            logo.Anchor = Anchor.TopLeft;
            logo.Origin = Anchor.Centre;
            logo.RelativePositionAxes = Axes.Both;
        }

        private void onExitingLogo()
        {
            logo?.AppendAnimatingAction(() => LogoExiting(logo), false);
        }

        /// <summary>Fired when this screen was exited to add any outwards transition to the logo.</summary>
        protected virtual void LogoExiting(RhythmicLogo logo)
        {
        }

        private void onSuspendingLogo()
        {
            logo?.AppendAnimatingAction(() => LogoSuspending(logo), false);
        }

        /// <summary>Fired when this screen was suspended to add any outwards transition to the logo.</summary>
        protected virtual void LogoSuspending(RhythmicLogo logo)
        {
        }

        /// <summary>Override to create a BackgroundMode for the current screen.
        /// Note that the instance created may not be the used instance if it matches the BackgroundMode equality clause.</summary>
        protected virtual BackgroundScreen CreateBackground() => null;

        public bool OnPressed(GlobalAction action)
        {
            if (!this.IsCurrentScreen()) return false;

            if (action == GlobalAction.Exit && AllowBackButton)
            {
                this.Exit();
                return true;
            }

            return false;
        }

        public bool OnReleased(GlobalAction action) => action == GlobalAction.Exit && AllowBackButton;
    }
}
