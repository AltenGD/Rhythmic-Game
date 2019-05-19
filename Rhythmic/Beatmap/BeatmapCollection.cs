using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace Rhythmic.Beatmap
{
    public class BeatmapCollection : Component
    {
        public BindableList<DatabasedBeatmap> Beatmaps { get; set; }

        public Bindable<DatabasedBeatmap> CurrentBeatmap { get; set; }
    }
}
