using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Rhythmic.Graphics.Containers
{
    public class RhythmicClickableContainer : ClickableContainer
    {
        private readonly Container content = new Container { RelativeSizeAxes = Axes.Both };

        protected override Container<Drawable> Content => content;

        [BackgroundDependencyLoader]
        private void load()
        {
            if (AutoSizeAxes != Axes.None)
            {
                content.RelativeSizeAxes = RelativeSizeAxes;
                content.AutoSizeAxes = AutoSizeAxes;
            }

            InternalChildren = new Drawable[]
            {
                content,
            };
        }
    }
}
