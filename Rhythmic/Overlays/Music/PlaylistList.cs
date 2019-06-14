using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Music
{
    public class PlaylistList : CompositeDrawable
    {
        public Action<BeatmapMeta> Selected;
        public Action<BeatmapMeta, int> OrderChanged;

        private readonly ItemsScrollContainer items;

        public PlaylistList()
        {
            InternalChild = items = new ItemsScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Selected = set => Selected?.Invoke(set),
                OrderChanged = (s, i) => OrderChanged?.Invoke(s, i)
            };
        }

        public new MarginPadding Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        public BeatmapMeta FirstVisibleSet => items.FirstVisibleSet;

        public void Filter(string searchTerm) => items.SearchTerm = searchTerm;

        private class ItemsScrollContainer : ScrollContainer<Drawable>
        {
            public Action<BeatmapMeta> Selected;
            public Action<BeatmapMeta, int> OrderChanged;

            private readonly SearchContainer search;
            private readonly FillFlowContainer<PlaylistItem> items;

            [Resolved]
            private BeatmapCollection collection { get; set; }

            public ItemsScrollContainer()
            {
                Children = new Drawable[]
                {
                    search = new SearchContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            items = new ItemSearchContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            },
                        }
                    }
                };
            }

            protected override void LoadComplete()
            {
                foreach (var b in collection.Beatmaps)
                    addBeatmapSet(b, false);

                collection.CurrentBeatmap.ValueChanged += _ => updateSelectedSet();
                base.LoadComplete();
            }

            private void addBeatmapSet(BeatmapMeta obj, bool existing) => Schedule(() =>
            {
                if (existing)
                    return;

                var newItem = new PlaylistItem(obj) { OnSelect = set => Selected?.Invoke(set) };

                items.Add(newItem);
                items.SetLayoutPosition(newItem, items.Count - 1);
            });

            private void updateSelectedSet()
            {
                foreach (PlaylistItem s in items.Children)
                    s.Selected = s.Beatmap.ID == collection.CurrentBeatmap.Value?.ID;
            }

            public string SearchTerm
            {
                get => search.SearchTerm;
                set => search.SearchTerm = value;
            }

            public BeatmapMeta FirstVisibleSet => items.FirstOrDefault(i => i.MatchingFilter)?.Beatmap;

            private Vector2 nativeDragPosition;
            private PlaylistItem draggedItem;

            protected override bool OnDragStart(DragStartEvent e)
            {
                nativeDragPosition = e.ScreenSpaceMousePosition;
                draggedItem = items.FirstOrDefault(d => d.IsDraggable);
                return draggedItem != null || base.OnDragStart(e);
            }

            protected override bool OnDrag(DragEvent e)
            {
                nativeDragPosition = e.ScreenSpaceMousePosition;
                if (draggedItem == null)
                    return base.OnDrag(e);

                return true;
            }

            protected override bool OnDragEnd(DragEndEvent e)
            {
                nativeDragPosition = e.ScreenSpaceMousePosition;
                var handled = draggedItem != null || base.OnDragEnd(e);
                draggedItem = null;

                return handled;
            }

            protected override void Update()
            {
                base.Update();

                if (draggedItem == null)
                    return;

                updateScrollPosition();
                updateDragPosition();
            }

            private void updateScrollPosition()
            {
                const float start_offset = 10;
                const double max_power = 50;
                const double exp_base = 1.05;

                var localPos = ToLocalSpace(nativeDragPosition);

                if (localPos.Y < start_offset)
                {
                    if (Current <= 0)
                        return;

                    var power = Math.Min(max_power, Math.Abs(start_offset - localPos.Y));
                    ScrollBy(-(float)Math.Pow(exp_base, power));
                }
                else if (localPos.Y > DrawHeight - start_offset)
                {
                    if (IsScrolledToEnd())
                        return;

                    var power = Math.Min(max_power, Math.Abs(DrawHeight - start_offset - localPos.Y));
                    ScrollBy((float)Math.Pow(exp_base, power));
                }
            }

            private void updateDragPosition()
            {
                var itemsPos = items.ToLocalSpace(nativeDragPosition);

                int srcIndex = (int)items.GetLayoutPosition(draggedItem);

                // Find the last item with position < mouse position. Note we can't directly use
                // the item positions as they are being transformed
                float heightAccumulator = 0;
                int dstIndex = 0;

                for (; dstIndex < items.Count; dstIndex++)
                {
                    // Using BoundingBox here takes care of scale, paddings, etc...
                    heightAccumulator += items[dstIndex].BoundingBox.Height;
                    if (heightAccumulator > itemsPos.Y)
                        break;
                }

                dstIndex = MathHelper.Clamp(dstIndex, 0, items.Count - 1);

                if (srcIndex == dstIndex)
                    return;

                if (srcIndex < dstIndex)
                {
                    for (int i = srcIndex + 1; i <= dstIndex; i++)
                        items.SetLayoutPosition(items[i], i - 1);
                }
                else
                {
                    for (int i = dstIndex; i < srcIndex; i++)
                        items.SetLayoutPosition(items[i], i + 1);
                }

                items.SetLayoutPosition(draggedItem, dstIndex);
                OrderChanged?.Invoke(draggedItem.Beatmap, dstIndex);
            }

            protected override ScrollbarContainer CreateScrollbar(Direction direction) => new PlaylistScrollbar(direction);

            protected class PlaylistScrollbar : ScrollbarContainer
            {
                private const float dim_size = 10;

                private Color4 hoverColour;
                private Color4 defaultColour;
                private Color4 highlightColour;

                private readonly Box box;

                public PlaylistScrollbar(Direction scrollDir)
                    : base(scrollDir)
                {
                    Blending = BlendingMode.Additive;

                    CornerRadius = 5;

                    const float margin = 3;

                    Margin = new MarginPadding
                    {
                        Left = scrollDir == Direction.Vertical ? margin : 0,
                        Right = scrollDir == Direction.Vertical ? margin : 0,
                        Top = scrollDir == Direction.Horizontal ? margin : 0,
                        Bottom = scrollDir == Direction.Horizontal ? margin : 0,
                    };

                    Masking = true;
                    Child = box = new Box { RelativeSizeAxes = Axes.Both };

                    ResizeTo(1);
                }

                [BackgroundDependencyLoader]
                private void load()
                {
                    Colour = defaultColour = RhythmicColors.Gray8;
                    hoverColour = RhythmicColors.GrayF;
                    highlightColour = RhythmicColors.Green;
                }

                public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
                {
                    Vector2 size = new Vector2(dim_size)
                    {
                        [(int)ScrollDirection] = val
                    };
                    this.ResizeTo(size, duration, easing);
                }

                protected override bool OnHover(HoverEvent e)
                {
                    this.FadeColour(hoverColour, 100);
                    return true;
                }

                protected override void OnHoverLost(HoverLostEvent e)
                {
                    this.FadeColour(defaultColour, 100);
                }

                protected override bool OnMouseDown(MouseDownEvent e)
                {
                    if (!base.OnMouseDown(e)) return false;

                    //note that we are changing the colour of the box here as to not interfere with the hover effect.
                    box.FadeColour(highlightColour, 100);
                    return true;
                }

                protected override bool OnMouseUp(MouseUpEvent e)
                {
                    if (e.Button != MouseButton.Left) return false;

                    box.FadeColour(Color4.White, 100);

                    return base.OnMouseUp(e);
                }
            }

            private class ItemSearchContainer : FillFlowContainer<PlaylistItem>, IHasFilterableChildren
            {
                public IEnumerable<string> FilterTerms => new string[] { };

                public bool MatchingFilter
                {
                    set
                    {
                        if (value)
                            InvalidateLayout();
                    }
                }

                public bool FilteringActive
                {
                    set { }
                }

                public IEnumerable<IFilterable> FilterableChildren => Children;

                public ItemSearchContainer()
                {
                    LayoutDuration = 200;
                    LayoutEasing = Easing.OutQuint;
                }
            }
        }
    }
}
