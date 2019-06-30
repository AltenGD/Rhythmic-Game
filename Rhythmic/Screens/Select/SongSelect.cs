using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Containers;
using Rhythmic.Screens.Backgrounds;
using System;
using System.Collections.Generic;

namespace Rhythmic.Screens.Select
{
    public abstract class SongSelect : RhythmicScreen, IKeyBindingHandler<GlobalAction>
    {
        private static readonly Vector2 wedged_container_size = new Vector2(0.5f, 245);

        protected const float BACKGROUND_BLUR = 20;
        private const float left_area_padding = 20;

        protected readonly BeatmapCarousel Carousel;

        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public override bool DisableBeatmapOnEnter => false;

        protected Bindable<BeatmapMeta> CurrentBeatmap = new Bindable<BeatmapMeta>();

        private readonly BeatmapInfoWedge beatmapInfoWedge;

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
                beatmapInfoWedge = new BeatmapInfoWedge
                {
                    Size = wedged_container_size,
                    RelativeSizeAxes = Axes.X,
                    Margin = new MarginPadding
                    {
                        Top = left_area_padding,
                        Right = left_area_padding,
                    },
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
            Carousel.LoadBeatmapSetsFromCollection(collection);

            CurrentBeatmap.ValueChanged += val =>
            {
                collection.CurrentBeatmap.Value = val.NewValue;
            };
        }

        /// <summary>
        /// Call to make a selection and perform the default action for this SongSelect.
        /// </summary>
        /// <param name="beatmap">An optional beatmap to override the current carousel selection.</param>
        /// <param name="performStartAction">Whether to trigger <see cref="OnStart"/>.</param>
        public void FinaliseSelection(BeatmapMeta beatmap = null, bool performStartAction = true)
        {
            // This is very important as we have not yet bound to screen-level bindables before the carousel load is completed.
            if (!Carousel.BeatmapSetsLoaded)
                return;

            // if we have a pending filter operation, we want to run it now.
            // it could change selection (ie. if the ruleset has been changed).
            Carousel.FlushPendingFilterOperations();

            // avoid attempting to continue before a selection has been obtained.
            // this could happen via a user interaction while the carousel is still in a loading state.
            if (Carousel.SelectedBeatmapSet == null) return;

            if (beatmap != null)
                Carousel.SelectBeatmap(beatmap);

            if (selectionChangedDebounce?.Completed == false)
            {
                selectionChangedDebounce.RunTask();
                selectionChangedDebounce.Cancel(); // cancel the already scheduled task.
                selectionChangedDebounce = null;
            }

            if (performStartAction)
                OnStart();
        }

        /// <summary>
        /// Called when a selection is made.
        /// </summary>
        /// <returns>If a resultant action occurred that takes the user away from SongSelect.</returns>
        protected abstract bool OnStart();

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

            boundLocalBindables = true;
        }

        private ScheduledDelegate selectionChangedDebounce;

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

                    //We want to make sure we stop the current track before we change to a new Beatmap.
                    collection.CurrentBeatmap.Value.Song.Stop();

                    collection.CurrentBeatmap.Value = beatmap;

                    if (this.IsCurrentScreen())
                        collection.CurrentBeatmap.Value.Song.Restart();
                }

                UpdateBeatmap(collection.CurrentBeatmap.Value);
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

            beatmapInfoWedge.Beatmap = beatmap;

            //BeatmapDetails.Beatmap = beatmap;

            if (beatmap.Song != null)
                beatmap.Song.Looping = true;
        }

        public override bool OnExiting(IScreen next)
        {
            if (base.OnExiting(next))
                return true;

            beatmapInfoWedge.Hide();

            this.FadeOut(100);

            if (collection.CurrentBeatmap.Value.Song != null)
                collection.CurrentBeatmap.Value.Song.Looping = false;

            return false;
        }

        public override void OnResuming(IScreen last)
        {
            collection.CurrentBeatmap.Disabled = false;
            base.OnResuming(last);
        }

        public virtual bool OnPressed(GlobalAction action)
        {
            if (!this.IsCurrentScreen()) return false;

            switch (action)
            {
                case GlobalAction.Select:
                    FinaliseSelection();
                    return true;
            }

            return false;
        }

        public bool OnReleased(GlobalAction action) => action == GlobalAction.Select;

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
