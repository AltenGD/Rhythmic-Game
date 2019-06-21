using System.ComponentModel;

namespace Rhythmic.Screens.Select
{
    public enum SortMode
    {
        [Description("Artist")]
        Artist,

        [Description("Author")]
        Author,

        [Description("BPM")]
        BPM,

        [Description("Date Added")]
        DateAdded,

        [Description("Difficulty")]
        Difficulty,

        [Description("Length")]
        Length,

        [Description("Rank Achieved")]
        RankAchieved,

        [Description("Title")]
        Title,

        [Description("Favorites")]
        Favorites,

        [Description("Recently Played")]
        RecentlyPlayed
    }
}
