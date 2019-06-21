using osu.Framework.Bindables;
using System;
using System.Collections.Generic;

namespace Rhythmic.Screens.Select.Carousel
{
    public abstract class CarouselItem
    {
        public readonly BindableBool Filtered = new BindableBool();

        public readonly Bindable<CarouselItemState> State = new Bindable<CarouselItemState>(CarouselItemState.NotSelected);

        /// <summary>
        /// This item is not in a hidden state.
        /// </summary>
        public bool Visible => State.Value != CarouselItemState.Collapsed && !Filtered.Value;

        public virtual List<DrawableCarouselItem> Drawables
        {
            get
            {
                List<DrawableCarouselItem> items = new List<DrawableCarouselItem>();

                DrawableCarouselItem self = DrawableRepresentation.Value;
                if (self?.IsPresent == true) items.Add(self);

                return items;
            }
        }

        protected CarouselItem()
        {
            DrawableRepresentation = new Lazy<DrawableCarouselItem>(CreateDrawableRepresentation);

            Filtered.ValueChanged += filtered =>
            {
                if (filtered.NewValue && State.Value == CarouselItemState.Selected)
                    State.Value = CarouselItemState.NotSelected;
            };
        }

        protected readonly Lazy<DrawableCarouselItem> DrawableRepresentation;

        /// <summary>
        /// Used as a default sort method for <see cref="CarouselItem"/>s of differing types.
        /// </summary>
        internal ulong ChildID;

        /// <summary>
        /// Create a fresh drawable version of this item. If you wish to consume the current representation, use <see cref="DrawableRepresentation"/> instead.
        /// </summary>
        protected abstract DrawableCarouselItem CreateDrawableRepresentation();

        public virtual void Filter(FilterCriteria criteria)
        {
        }

        public virtual int CompareTo(FilterCriteria criteria, CarouselItem other) => ChildID.CompareTo(other.ChildID);
    }

    public enum CarouselItemState
    {
        Collapsed,
        NotSelected,
        Selected,
    }
}
