using osu.Framework.Bindables;
using osu.Framework.Graphics;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Beatmap
{
    public class BeatmapCollection : Component
    {
        public BindableList<BeatmapMeta> Beatmaps { get; set; }

        public Bindable<BeatmapMeta> CurrentBeatmap { get; set; }
    }
}
