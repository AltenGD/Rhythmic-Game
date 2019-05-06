using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System.Collections.Generic;

namespace Rhythmic.Beatmap
{
    public class BeatmapCollection : Component
    {
        public List<DatabasedBeatmap> Beatmaps { get; set; }

        public Bindable<DatabasedBeatmap> CurrentBeatmap { get; set; }
    }
}
