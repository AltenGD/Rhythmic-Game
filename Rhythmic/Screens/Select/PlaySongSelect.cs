using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Input;
using Rhythmic.Beatmap;

namespace Rhythmic.Screens.Select
{
    public class PlaySongSelect : SongSelect
    {
        private RhythmicScreen player;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        public override void OnResuming(IScreen last)
        {
            player = null;

            base.OnResuming(last);
        }

        protected override bool OnStart()
        {
            if (player != null) return false;

            collection.CurrentBeatmap.Value.Song.Looping = false;

            LoadComponentAsync(player = new Play.Play(), l =>
            {
                if (this.IsCurrentScreen()) this.Push(player);
            });

            return true;
        }
    }
}
