using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osu.Framework.Allocation;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Drawables;
using System;

namespace Rhythmic.Screens.Select.Components
{
    public class BeatmapLevelListing : DrawSizePreservingFillContainer
    {
        private FillFlowContainer beatmapContainer;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public Action selectedBeatmap;

        public BeatmapLevelListing()
        {
            RelativeSizeAxes = Axes.Both;
            Strategy = DrawSizePreservationStrategy.Minimum;

            Children = new Drawable[]
            {
                new ScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Top = 100,
                        Horizontal = 10,
                        Bottom = 10
                    },
                    Children = new Drawable[]
                    {
                        beatmapContainer = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Full,
                            Spacing = new Vector2(10),
                            LayoutDuration = 100,
                            LayoutEasing = Easing.Out,
                        }
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            foreach (var beatmap in collection.Beatmaps)
                beatmapContainer.Add(new BeatmapCard(beatmap)
                {
                    Action = delegate
                    {
                        collection.CurrentBeatmap.Value = beatmap;
                        selectedBeatmap?.Invoke();
                    }
                });
        }
    }
}
