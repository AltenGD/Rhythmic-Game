using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Screens.Edit
{
    public class EditorScreen : Container
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected IBindable<BeatmapMeta> Beatmap => collection.CurrentBeatmap;

        protected override Container<Drawable> Content => content;
        private readonly Container content;

        public EditorScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            InternalChild = content = new Container { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            this.FadeTo(0)
                .Then()
                .FadeTo(1f, 250, Easing.OutQuint);
        }

        public void Exit()
        {
            this.FadeOut(250).Expire();
        }
    }
}
