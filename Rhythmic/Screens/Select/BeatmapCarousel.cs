using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Threading;
using osuTK;
using osuTK.Input;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Containers;
using Rhythmic.Screens.Select.Carousel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rhythmic.Screens.Select
{
    public class BeatmapCarousel : RhythmicScrollContainer
    {
        /// <summary>Triggered when the <see cref="BeatmapMeta"/> loaded change and are completely loaded.</summary>
        public Action BeatmapSetsChanged;

        /// <summary>The currently selected beatmap set.</summary>
        public BeatmapMeta SelectedBeatmapSet => selectedBeatmapSet?.BeatmapSet;

        private CarouselBeatmapSet selectedBeatmapSet;

        /// <summary>Raised when the <see cref="SelectedBeatmap"/> is changed.</summary>
        public Action<BeatmapMeta> SelectionChanged;

        public override bool HandleNonPositionalInput => AllowSelection;
        public override bool HandlePositionalInput => AllowSelection;

        /// <summary>Whether carousel items have completed asynchronously loaded.</summary>
        public bool BeatmapSetsLoaded { get; private set; }

        private IEnumerable<CarouselBeatmapSet> beatmapSets => root.Children.OfType<CarouselBeatmapSet>();

        public IEnumerable<BeatmapMeta> BeatmapSets
        {
            get => beatmapSets.Select(g => g.BeatmapSet);
            set => loadBeatmapSets(value);
        }


        public void LoadBeatmapSetsFromCollection(BeatmapCollection manager) => loadBeatmapSets(manager.Beatmaps);

        private void loadBeatmapSets(IEnumerable<BeatmapMeta> beatmapSets)
        {
            CarouselRoot newRoot = new CarouselRoot(this);

            Task.Run(() =>
            {
                beatmapSets.Select(createCarouselSet).Where(g => g != null).ForEach(newRoot.AddChild);
                newRoot.Filter(activeCriteria);

                // preload drawables as the ctor overhead is quite high currently.
                List<DrawableCarouselItem> _ = newRoot.Drawables;
            }).ContinueWith(_ => Schedule(() =>
            {
                root = newRoot;
                scrollableContent.Clear(false);
                itemsCache.Invalidate();
                scrollPositionCache.Invalidate();

                Schedule(() =>
                {
                    BeatmapSetsChanged?.Invoke();
                    BeatmapSetsLoaded = true;
                });
            }));
        }

        private readonly List<float> yPositions = new List<float>();
        private Cached itemsCache = new Cached();
        private Cached scrollPositionCache = new Cached();

        private readonly Container<DrawableCarouselItem> scrollableContent;

        public Bindable<bool> RightClickScrollingEnabled = new Bindable<bool>();

        public Bindable<RandomSelectAlgorithm> RandomAlgorithm = new Bindable<RandomSelectAlgorithm>();
        private readonly List<CarouselBeatmapSet> previouslyVisitedRandomSets = new List<CarouselBeatmapSet>();
        private readonly Stack<CarouselBeatmapSet> randomSelectedBeatmaps = new Stack<CarouselBeatmapSet>();

        protected List<DrawableCarouselItem> Items = new List<DrawableCarouselItem>();
        private CarouselRoot root;

        public BeatmapCarousel()
        {
            root = new CarouselRoot(this);
            Child = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Child = scrollableContent = new Container<DrawableCarouselItem>
                {
                    RelativeSizeAxes = Axes.X
                }
            };
        }

        public void RemoveBeatmapSet(BeatmapMeta beatmapSet)
        {
            Schedule(() =>
            {
                CarouselBeatmapSet existingSet = beatmapSets.FirstOrDefault(b => b.BeatmapSet.ID == beatmapSet.ID);

                if (existingSet == null)
                    return;

                root.RemoveChild(existingSet);
                itemsCache.Invalidate();
            });
        }

        public void UpdateBeatmapSet(BeatmapMeta beatmapSet)
        {
            Schedule(() =>
            {
                int? previouslySelectedID = null;
                CarouselBeatmapSet existingSet = beatmapSets.FirstOrDefault(b => b.BeatmapSet.ID == beatmapSet.ID);

                // If the selected beatmap is about to be removed, store its ID so it can be re-selected if required
                if (existingSet?.State?.Value == CarouselItemState.Selected)
                    previouslySelectedID = selectedBeatmapSet?.BeatmapSet?.ID;

                CarouselBeatmapSet newSet = createCarouselSet(beatmapSet);

                if (existingSet != null)
                    root.RemoveChild(existingSet);

                if (newSet == null)
                {
                    itemsCache.Invalidate();
                    return;
                }

                root.AddChild(newSet);

                applyActiveCriteria(false, false);

                itemsCache.Invalidate();
                Schedule(() => BeatmapSetsChanged?.Invoke());
            });
        }

        /// <summary>Selects a given beatmap on the carousel.
        ///
        /// If bypassFilters is false, we will try to select another unfiltered beatmap in the same set. If the
        /// entire set is filtered, no selection is made.</summary>
        /// <param name="beatmap">The beatmap to select.</param>
        /// <param name="bypassFilters">Whether to select the beatmap even if it is filtered (i.e., not visible on carousel).</param>
        /// <returns>True if a selection was made, False if it wasn't.</returns>
        public bool SelectBeatmap(BeatmapMeta beatmap, bool bypassFilters = true)
        {
            foreach (CarouselBeatmapSet set in beatmapSets)
            {
                if (!bypassFilters && set.Filtered.Value)
                    continue;

                CarouselBeatmapSet item = set;

                if (item == null)
                    // The beatmap that needs to be selected doesn't exist in this set
                    continue;

                if (!bypassFilters && item.Filtered.Value)
                    // The beatmap exists in this set but is filtered, so look for the first unfiltered map in the set
                    item = set;

                if (item != null)
                {
                    select(item);
                    return true;
                }
            }

            return false;
        }

        /// <summary>Increment selection in the carousel in a chosen direction.</summary>
        /// <param name="direction">The direction to increment. Negative is backwards.</param>
        /// <param name="skipDifficulties">Whether to skip individual difficulties and only increment over full groups.</param>
        public void SelectNext(int direction = 1)
        {
            List<DrawableCarouselItem> visibleItems = Items.Where(s => !s.Item.Filtered.Value).ToList();

            if (!visibleItems.Any())
                return;

            DrawableCarouselItem drawable = null;

            if (selectedBeatmapSet != null && (drawable = selectedBeatmapSet.Drawables.FirstOrDefault()) == null)
                // if the selected beatmap isn't present yet, we can't correctly change selection.
                // we can fix this by changing this method to not reference drawables / Items in the first place.
                return;

            int originalIndex = visibleItems.IndexOf(drawable);
            int currentIndex = originalIndex;

            // local function to increment the index in the required direction, wrapping over extremities.
            int incrementIndex() => currentIndex = (currentIndex + direction + visibleItems.Count) % visibleItems.Count;

            while (incrementIndex() != originalIndex)
            {
                CarouselItem item = visibleItems[currentIndex].Item;

                if (item.Filtered.Value || item.State.Value == CarouselItemState.Selected) continue;

                switch (item)
                {
                    case CarouselBeatmapSet set:
                        select(set);
                        return;
                }
            }
        }

        private void select(CarouselItem item)
        {
            if (!AllowSelection)
                return;

            if (item == null) return;

            item.State.Value = CarouselItemState.Selected;
        }

        private FilterCriteria activeCriteria = new FilterCriteria();

        protected ScheduledDelegate PendingFilter;

        public bool AllowSelection = true;

        public void FlushPendingFilterOperations()
        {
            if (PendingFilter?.Completed == false)
            {
                applyActiveCriteria(false, false);
                Update();
            }
        }

        public void Filter(FilterCriteria newCriteria, bool debounce = true)
        {
            if (newCriteria != null)
                activeCriteria = newCriteria;

            applyActiveCriteria(debounce, true);
        }

        private void applyActiveCriteria(bool debounce, bool scroll)
        {
            if (root.Children.Any() != true) return;

            void perform()
            {
                PendingFilter = null;

                root.Filter(activeCriteria);
                itemsCache.Invalidate();
                if (scroll) scrollPositionCache.Invalidate();
            }

            PendingFilter?.Cancel();
            PendingFilter = null;

            if (debounce)
                PendingFilter = Scheduler.AddDelayed(perform, 250);
            else
                perform();
        }

        private float? scrollTarget;

        public void ScrollToSelected() => scrollPositionCache.Invalidate();

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            int direction = 0;

            switch (e.Key)
            {
                case Key.Up:
                    direction = -1;
                    break;

                case Key.Down:
                    direction = 1;
                    break;

                case Key.Left:
                    direction = -1;
                    break;

                case Key.Right:
                    direction = 1;
                    break;
            }

            if (direction == 0)
                return base.OnKeyDown(e);

            SelectNext(direction);
            return true;
        }

        protected override void Update()
        {
            base.Update();

            if (!itemsCache.IsValid)
                updateItems();

            if (!scrollPositionCache.IsValid)
                updateScrollPosition();

            float drawHeight = DrawHeight;

            // Remove all items that should no longer be on-screen
            scrollableContent.RemoveAll(p => p.Y < Current - p.DrawHeight || p.Y > Current + drawHeight || !p.IsPresent);

            // Find index range of all items that should be on-screen
            Trace.Assert(Items.Count == yPositions.Count);

            int firstIndex = yPositions.BinarySearch(Current - DrawableCarouselItem.MAX_HEIGHT);
            if (firstIndex < 0) firstIndex = ~firstIndex;
            int lastIndex = yPositions.BinarySearch(Current + drawHeight);
            if (lastIndex < 0) lastIndex = ~lastIndex;

            int notVisibleCount = 0;

            // Add those items within the previously found index range that should be displayed.
            for (int i = firstIndex; i < lastIndex; ++i)
            {
                DrawableCarouselItem item = Items[i];

                if (!item.Item.Visible)
                {
                    if (!item.IsPresent)
                        notVisibleCount++;
                    continue;
                }

                float depth = i + (item is DrawableCarouselBeatmapSet ? -Items.Count : 0);

                // Only add if we're not already part of the content.
                if (!scrollableContent.Contains(item))
                {
                    // Makes sure headers are always _below_ items,
                    // and depth flows downward.
                    item.Depth = depth;

                    switch (item.LoadState)
                    {
                        case LoadState.NotLoaded:
                            LoadComponentAsync(item);
                            break;

                        case LoadState.Loading:
                            break;

                        default:
                            scrollableContent.Add(item);
                            break;
                    }
                }
                else
                {
                    scrollableContent.ChangeChildDepth(item, depth);
                }
            }

            // this is not actually useful right now, but once we have groups may well be.
            if (notVisibleCount > 50)
                itemsCache.Invalidate();

            // Update externally controlled state of currently visible items
            // (e.g. x-offset and opacity).
            float halfHeight = drawHeight / 2;
            foreach (DrawableCarouselItem p in scrollableContent.Children)
                updateItem(p, halfHeight);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            // aggressively dispose "off-screen" items to reduce GC pressure.
            foreach (DrawableCarouselItem i in Items)
                i.Dispose();
        }

        private CarouselBeatmapSet createCarouselSet(BeatmapMeta beatmapSet)
        {
            CarouselBeatmapSet set = new CarouselBeatmapSet(beatmapSet);

            set.State.ValueChanged += state =>
            {
                Console.WriteLine($"set: {set.BeatmapSet.ToString()} occured: {state.NewValue}");
                if (state.NewValue == CarouselItemState.Selected)
                {
                    selectedBeatmapSet = set;
                    SelectionChanged?.Invoke(beatmapSet);

                    itemsCache.Invalidate();
                    scrollPositionCache.Invalidate();
                }
            };

            return set;
        }

        /// <summary>Computes the target Y positions for every item in the carousel.</summary>
        /// <returns>The Y position of the currently selected item.</returns>
        private void updateItems()
        {
            Items = root.Drawables.ToList();

            yPositions.Clear();

            float currentY = DrawHeight / 2;
            DrawableCarouselBeatmapSet lastSet = null;

            scrollTarget = null;

            foreach (DrawableCarouselItem d in Items)
            {
                if (d.IsPresent)
                {
                    switch (d)
                    {
                        case DrawableCarouselBeatmapSet set:
                            lastSet = set;

                            set.MoveToX(set.Item.State.Value == CarouselItemState.Selected ? -100 : 0, 500, Easing.OutExpo);
                            set.MoveToY(currentY, 750, Easing.OutExpo);
                            break;
                    }
                }

                yPositions.Add(currentY);

                if (d.Item.Visible)
                    currentY += d.DrawHeight + 5;
            }

            currentY += DrawHeight / 2;
            scrollableContent.Height = currentY;

            if (BeatmapSetsLoaded && (selectedBeatmapSet == null || selectedBeatmapSet.State.Value != CarouselItemState.Selected))
            {
                selectedBeatmapSet = null;
                SelectionChanged?.Invoke(null);
            }

            itemsCache.Validate();
        }

        private void updateScrollPosition()
        {
            if (scrollTarget != null) ScrollTo(scrollTarget.Value);
            scrollPositionCache.Validate();
        }

        /// <summary>Computes the x-offset of currently visible items. Makes the carousel appear round.</summary>
        /// <param name="dist">
        /// Vertical distance from the center of the carousel container
        /// ranging from -1 to 1.
        /// </param>
        /// <param name="halfHeight">Half the height of the carousel container.</param>
        private static float offsetX(float dist, float halfHeight)
        {
            // The radius of the circle the carousel moves on.
            const float circle_radius = 3;
            double discriminant = Math.Max(0, circle_radius * circle_radius - dist * dist);
            float x = (circle_radius - (float)Math.Sqrt(discriminant)) * halfHeight;

            return 125 + x;
        }

        /// <summary>Update a item's x position and multiplicative alpha based on its y position and
        /// the current scroll position.</summary>
        /// <param name="p">The item to be updated.</param>
        /// <param name="halfHeight">Half the draw height of the carousel container.</param>
        private void updateItem(DrawableCarouselItem p, float halfHeight)
        {
            float height = p.IsPresent ? p.DrawHeight : 0;

            float itemDrawY = p.Position.Y - Current + height / 2;
            float dist = Math.Abs(1f - itemDrawY / halfHeight);

            // Setting the origin position serves as an additive position on top of potential
            // local transformation we may want to apply (e.g. when a item gets selected, we
            // may want to smoothly transform it leftwards.)
            p.OriginPosition = new Vector2(-offsetX(dist, halfHeight), 0);

            // We are applying a multiplicative alpha (which is internally done by nesting an
            // additional container and setting that container's alpha) such that we can
            // layer transformations on top, with a similar reasoning to the previous comment.
            p.SetMultiplicativeAlpha(MathHelper.Clamp(1.75f - 1.5f * dist, 0, 1));
        }

        private class CarouselRoot : CarouselGroupEagerSelect
        {
            private readonly BeatmapCarousel carousel;

            public CarouselRoot(BeatmapCarousel carousel)
            {
                this.carousel = carousel;
            }

            protected override void PerformSelection()
            {
                if (LastSelected != null)
                    base.PerformSelection();
            }
        }
    }
}
