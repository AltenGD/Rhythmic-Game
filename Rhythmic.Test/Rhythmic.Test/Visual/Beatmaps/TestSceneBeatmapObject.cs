using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using osu.Framework.Testing;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework;
using System.IO;
using Rhythmic.Beatmap.Drawables;

namespace Rhythmic.Test.Visual.Beatmaps
{
    public class TestSceneBeatmapObject : TestScene
    {
        private BeatmapMeta TestLevel;
        private BeatmapAPI API;
        private TrackBass track;

        [BackgroundDependencyLoader]
        private void load(Game game, AudioManager Audio)
        {
            API = new BeatmapAPI();

            TestLevel = API.ParseBeatmap(File.ReadAllText("../../../../../Rhythmic.Resources/Beatmap/NULCTRL/level.json"));

            track = new TrackBass(game.Resources.GetStream("Beatmap/NULCTRL/song.mp3"));
            Audio.AddItem(track);

            AddStep("aa", null);
            AddStep("Restart", () =>
            {
                Clear();

                track.Stop();

                foreach (var o in TestLevel.Level.Level)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Add(new DrawableBeatmapObject(o));
                    }, o.Time);
                }
            });

            AddStep("Restart w/song", () =>
            {
                Clear();

                track.Stop();
                track.Seek(0);
                track.Start();

                foreach (var o in TestLevel.Level.Level)
                {
                    Scheduler.AddDelayed(() =>
                    {
                        Add(new DrawableBeatmapObject(o));
                    }, o.Time);
                }
            });
        }

        protected override void LoadComplete()
        {
            foreach (var o in TestLevel.Level.Level)
            {
                Scheduler.AddDelayed(() =>
                {
                    Add(new DrawableBeatmapObject(o));
                }, o.Time);
            }

            base.LoadComplete();
        }
    }
}