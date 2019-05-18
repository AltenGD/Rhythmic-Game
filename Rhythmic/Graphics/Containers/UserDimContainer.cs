using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.Containers
{
    /// <summary> A container that applies user-configured visual settings to its contents.
    /// This container specifies behavior that applies to Backgrounds. </summary>
    public class UserDimContainer : Container
    {
        private const float background_fade_duration = 800;

        /// <summary>Whether or not user-configured dim levels should be applied to the container.</summary>
        public readonly Bindable<bool> EnableUserDim = new Bindable<bool>();

        /// <summary>The amount of blur to be applied to the background in addition to user-specified blur.</summary>
        public readonly Bindable<float> BlurAmount = new Bindable<float>();

        private Bindable<double> userDimLevel { get; set; } = new Bindable<double>(0.5);

        private Bindable<double> userBlurLevel { get; set; } = new Bindable<double>(20);

        protected Container DimContainer { get; }

        protected override Container<Drawable> Content => DimContainer;

        /// <summary>As an optimisation, we add the two blur portions to be applied rather than actually applying two separate blurs.</summary>
        private Vector2 blurTarget => EnableUserDim.Value
            ? new Vector2(BlurAmount.Value + (float)userBlurLevel.Value * 25)
            : new Vector2(BlurAmount.Value);

        /// <summary>Creates a new <see cref="UserDimContainer"/>.</summary>
        public UserDimContainer()
        {
            AddInternal(DimContainer = new Container { RelativeSizeAxes = Axes.Both });
        }

        private Background background;

        public Background Background
        {
            get => background;
            set
            {
                base.Add(background = value);
                background.BlurTo(blurTarget, 0, Easing.OutQuint);
            }
        }

        public override void Add(Drawable drawable)
        {
            if (drawable is Background)
                throw new InvalidOperationException($"Use {nameof(Background)} to set a background.");

            base.Add(drawable);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            /*userDimLevel = config.GetBindable<double>(OsuSetting.DimLevel);
            userBlurLevel = config.GetBindable<double>(OsuSetting.BlurLevel);

            EnableUserDim.ValueChanged += _ => updateVisuals();
            userDimLevel.ValueChanged += _ => updateVisuals();
            userBlurLevel.ValueChanged += _ => updateVisuals();
            BlurAmount.ValueChanged += _ => updateVisuals();*/
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            updateVisuals();
        }
        private void updateVisuals()
        {
            Background?.BlurTo(blurTarget, background_fade_duration, Easing.OutQuint);

            DimContainer.FadeColour(EnableUserDim.Value ? RhythmicColors.Gray(1 - (float)userDimLevel.Value) : Color4.White, background_fade_duration, Easing.OutQuint);
        }
    }
}
