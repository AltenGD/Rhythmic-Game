using Rhythmic.Beatmap.Properties;
using System;

namespace Rhythmic.Screens.Select.Carousel
{
    public class CarouselBeatmapSet : CarouselItem
    {
        public BeatmapMeta BeatmapSet;

        public CarouselBeatmapSet(BeatmapMeta beatmapSet)
        {
            BeatmapSet = beatmapSet ?? throw new ArgumentNullException(nameof(beatmapSet));
        }

        protected override DrawableCarouselItem CreateDrawableRepresentation() => new DrawableCarouselBeatmapSet(this);

        public override int CompareTo(FilterCriteria criteria, CarouselItem other)
        {
            if (!(other is CarouselBeatmapSet otherSet))
                return base.CompareTo(criteria, other);

            switch (criteria.Sort)
            {
                default:
                case SortMode.Artist:
                    return string.Compare(BeatmapSet.Metadata.Song.Author, otherSet.BeatmapSet.Metadata.Song.Author, StringComparison.InvariantCultureIgnoreCase);

                case SortMode.Title:
                    return string.Compare(BeatmapSet.Metadata.Song.Name, otherSet.BeatmapSet.Metadata.Song.Name, StringComparison.InvariantCultureIgnoreCase);

                case SortMode.Author:
                    return string.Compare(BeatmapSet.Metadata.Level.CreatorName, otherSet.BeatmapSet.Metadata.Level.CreatorName, StringComparison.InvariantCultureIgnoreCase);

                case SortMode.DateAdded:
                    return otherSet.BeatmapSet.DownloadedAt.CompareTo(BeatmapSet.DownloadedAt);
            }
        }

        public override string ToString() => BeatmapSet.ToString();
    }
}
