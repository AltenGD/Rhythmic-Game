using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Screens.Backgrounds;
using System;
using System.Collections.Generic;

namespace Rhythmic.Screens.Select
{
    public class SongSelect : RhythmicScreen
    {
        protected readonly BeatmapCarousel Carousel;

        protected const float BACKGROUND_BLUR = 20;

        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public override bool DisableBeatmapOnEnter => false;

        protected Bindable<BeatmapMeta> CurrentBeatmap = new Bindable<BeatmapMeta>();

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public SongSelect()
        {
            AddRangeInternal(new Drawable[]
            {
                Carousel = new BeatmapCarousel
                {
                    Masking = false,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1 - 0.5f, 1),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    SelectionChanged = updateSelectedBeatmap,
                    BeatmapSetsChanged = carouselBeatmapsLoaded,
                },
                new ResetScrollContainer(() => Carousel.ScrollToSelected())
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 250,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            List<BeatmapMeta> list = new List<BeatmapMeta>()
            {
                new BeatmapMeta
                {
                    Metadata = new BeatmapMetadata
                    {
                        Song = new SongMetadata
                        {
                            Author = "Test",
                            Name = "Test"
                        }
                    }
                },
                new BeatmapMeta
                {
                    Metadata = new BeatmapMetadata
                    {
                        Song = new SongMetadata
                        {
                            Author = "Test1",
                            Name = "Test1"
                        }
                    }
                },
                new BeatmapMeta
                {
                    Metadata = new BeatmapMetadata
                    {
                        Song = new SongMetadata
                        {
                            Author = "Test2",
                            Name = "Test2"
                        }
                    }
                },
                new BeatmapMeta
                {
                    Metadata = new BeatmapMetadata
                    {
                        Song = new SongMetadata
                        {
                            Author = "Test3",
                            Name = "Test3"
                        }
                    }
                },
            };

            //Carousel.BeatmapSets = list;
            Carousel.LoadBeatmapSetsFromCollection(collection);

            CurrentBeatmap.ValueChanged += val =>
            {
                collection.CurrentBeatmap.Value = val.NewValue;
            };
        }

        private BeatmapMeta beatmapNoDebounce;

        private void updateSelectedBeatmap(BeatmapMeta beatmap)
        {
            if (beatmap?.Equals(beatmapNoDebounce) == true)
                return;

            beatmapNoDebounce = beatmap;
            performUpdateSelected();
        }

        private void carouselBeatmapsLoaded()
        {
            bindBindables();

            // If a selection was already obtained, do not attempt to update the selected beatmap.
            if (Carousel.SelectedBeatmapSet != null)
                return;

            // Attempt to select the current beatmap on the carousel, if it is valid to be selected.
            if (!collection.CurrentBeatmap.IsDefault && Carousel.SelectBeatmap(collection.CurrentBeatmap.Value, false))
                return;
        }

        private bool boundLocalBindables;

        private void bindBindables()
        {
            if (boundLocalBindables)
                return;

            collection.CurrentBeatmap.BindDisabledChanged(disabled => Carousel.AllowSelection = !disabled, true);
            collection.CurrentBeatmap.BindValueChanged(workingBeatmapChanged);

            boundLocalBindables = true;
        }

        private ScheduledDelegate selectionChangedDebounce;

        private void workingBeatmapChanged(ValueChangedEvent<BeatmapMeta> e)
        {
            if (this.IsCurrentScreen() && !Carousel.SelectBeatmap(e.NewValue, false))
                Carousel.SelectBeatmap(e.NewValue);
        }

        /// <summary>selection has been changed as the result of a user interaction.</summary>
        private void performUpdateSelected()
        {
            BeatmapMeta beatmap = beatmapNoDebounce;

            selectionChangedDebounce?.Cancel();

            if (beatmap == null)
                run();
            else
                selectionChangedDebounce = Scheduler.AddDelayed(run, 200);

            void run()
            {
                Logger.Log($"updating selection with beatmap:{beatmap?.ID.ToString() ?? "null"}");

                // We may be arriving here due to another component changing the bindable Beatmap.
                // In these cases, the other component has already loaded the beatmap, so we don't need to do so again.
                if (!Equals(beatmap, collection.CurrentBeatmap.Value))
                {
                    Logger.Log($"beatmap changed from \"{collection.CurrentBeatmap.Value}\" to \"{beatmap}\"");

                    CurrentBeatmap.Value = beatmap;
                }

                if (this.IsCurrentScreen())
                    ensurePlayingSelected();

                UpdateBeatmap(beatmap);
            }
        }

        /// <summary>Allow components in SongSelect to update their loaded beatmap details.
        /// This is a debounced call (unlike directly binding to WorkingBeatmap.ValueChanged).</summary>
        /// <param name="beatmap">The working beatmap.</param>
        protected virtual void UpdateBeatmap(BeatmapMeta beatmap)
        {
            Logger.Log($"working beatmap updated to {beatmap}");

            if (Background is BackgroundScreenBeatmap backgroundModeBeatmap)
            {
                backgroundModeBeatmap.Beatmap = beatmap;
                backgroundModeBeatmap.BlurAmount.Value = BACKGROUND_BLUR;
                backgroundModeBeatmap.FadeColour(Color4.White, 250);
            }

            //beatmapInfoWedge.Beatmap = beatmap;

            //BeatmapDetails.Beatmap = beatmap;

            if (beatmap.Song != null)
                beatmap.Song.Looping = true;
        }

        private void ensurePlayingSelected(bool restart = false)
        {
            if (!CurrentBeatmap.Value.Song.IsRunning || restart)
            {
                //track.RestartPoint = Beatmap.Value.Metadata.PreviewTime;
                CurrentBeatmap.Value.Song.Restart();
            }
        }

        public override void OnResuming(IScreen last)
        {
            collection.CurrentBeatmap.Disabled = false;
            base.OnResuming(last);
        }

        private class ResetScrollContainer : Container
        {
            private readonly Action onHoverAction;

            public ResetScrollContainer(Action onHoverAction)
            {
                this.onHoverAction = onHoverAction;
            }

            protected override bool OnHover(HoverEvent e)
            {
                onHoverAction?.Invoke();
                return base.OnHover(e);
            }
        }
    }
}
