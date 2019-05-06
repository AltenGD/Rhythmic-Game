using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using Rhythmic.Beatmap;

namespace Rhythmic.Visualizers
{
    public abstract class MusicVisualizerContainer : Container
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        private readonly Bindable<DatabasedBeatmap> beatmap = new Bindable<DatabasedBeatmap>();

        private int updateDelay = 1;
        public int UpdateDelay
        {
            set
            {
                if (updateDelay == value)
                    return;
                updateDelay = value;

                if (!IsLoaded)
                    return;

                Restart();
            }
            get { return updateDelay; }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            beatmap.Value = collection.CurrentBeatmap.Value;

            collection.CurrentBeatmap.ValueChanged += delegate 
            {
                beatmap.Value = collection.CurrentBeatmap.Value;
                System.Console.WriteLine("Changed!");
                Restart();
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Start();
        }

        private void updateAmplitudes()
        {
            var frequencyAmplitudes = beatmap.Value.Song?.CurrentAmplitudes.FrequencyAmplitudes ?? new float[256];
            OnAmplitudesUpdate(frequencyAmplitudes);
            Scheduler.AddDelayed(updateAmplitudes, UpdateDelay);
        }

        protected void Start() => updateAmplitudes();

        protected abstract void OnAmplitudesUpdate(float[] amplitudes);

        protected void Restart()
        {
            Scheduler.CancelDelayedTasks();
            Start();
        }
    }
}
