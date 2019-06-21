using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Visualizers
{
    public abstract class MusicVisualizerContainer : Container
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        private readonly Bindable<BeatmapMeta> beatmap = new Bindable<BeatmapMeta>();

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
            beatmap.Value = collection?.CurrentBeatmap?.Value;

            collection.CurrentBeatmap.ValueChanged += val =>
            {
                beatmap.Value = val.NewValue;
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
            float[] frequencyAmplitudes = beatmap.Value?.Song?.CurrentAmplitudes.FrequencyAmplitudes ?? new float[256];
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
