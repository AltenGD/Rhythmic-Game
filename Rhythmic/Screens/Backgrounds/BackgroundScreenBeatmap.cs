using System.Threading;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using Rhythmic.Beatmap;
using Rhythmic.Graphics;
using Rhythmic.Graphics.Containers;

namespace Rhythmic.Screens.Backgrounds
{
    public class BackgroundScreenBeatmap : BackgroundScreen
    {
        protected Background Background;

        private DatabasedBeatmap beatmap;

        /// <summary>Whether or not user dim settings should be applied to this Background.</summary>
        public readonly Bindable<bool> EnableUserDim = new Bindable<bool>();

        /// <summary>The amount of blur to be applied in addition to user-specified blur.</summary>
        public readonly Bindable<float> BlurAmount = new Bindable<float>();

        private readonly UserDimContainer fadeContainer;

        protected virtual UserDimContainer CreateFadeContainer() => new UserDimContainer { RelativeSizeAxes = Axes.Both };

        public BackgroundScreenBeatmap(DatabasedBeatmap beatmap = null)
        {
            Beatmap = beatmap;
            InternalChild = fadeContainer = CreateFadeContainer();
            fadeContainer.EnableUserDim.BindTo(EnableUserDim);
            fadeContainer.BlurAmount.BindTo(BlurAmount);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var background = new BeatmapBackground(beatmap);
            LoadComponent(background);
            switchBackground(background);
        }

        private CancellationTokenSource cancellationSource;

        public DatabasedBeatmap Beatmap
        {
            get => beatmap;
            set
            {
                if (beatmap == value && beatmap != null)
                    return;

                beatmap = value;

                Schedule(() =>
                {
                    if ((Background as BeatmapBackground)?.Beatmap == beatmap)
                        return;

                    cancellationSource?.Cancel();
                    LoadComponentAsync(new BeatmapBackground(beatmap), switchBackground, (cancellationSource = new CancellationTokenSource()).Token);
                });
            }
        }

        private void switchBackground(BeatmapBackground b)
        {
            float newDepth = 0;

            if (Background != null)
            {
                newDepth = Background.Depth + 1;
                Background.FinishTransforms();
                Background.FadeOut(250);
                Background.Expire();
            }

            b.Depth = newDepth;
            fadeContainer.Background = Background = b;
        }

        public override bool Equals(BackgroundScreen other)
        {
            if (!(other is BackgroundScreenBeatmap otherBeatmapBackground)) return false;

            return base.Equals(other) && beatmap == otherBeatmapBackground.Beatmap;
        }

        protected class BeatmapBackground : Background
        {
            public readonly DatabasedBeatmap Beatmap;

            public BeatmapBackground(DatabasedBeatmap beatmap)
            {
                Beatmap = beatmap;
            }

            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                Sprite.Texture = Beatmap?.Background ?? textures.Get(@"Backgrounds/bg1");
            }
        }
    }
}
