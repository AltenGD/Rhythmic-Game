using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;

namespace Rhythmic.Screens.Play
{
    /// <summary>Encapsulates gameplay timing logic and provides a <see cref="Play.GameplayClock"/> for children.</summary>
    public class GameplayClockContainer : Container
    {
        private readonly BeatmapMeta beatmap;

        /// <summary>The original source (usually a <see cref="DatabasedBeatmap"/>'s track).</summary>
        private readonly IAdjustableClock sourceClock;

        public readonly BindableBool IsPaused = new BindableBool();

        /// <summary>The decoupled clock used for gameplay. Should be used for seeks and clock control.</summary>
        private readonly DecoupleableInterpolatingFramedClock adjustableClock;

        private readonly double gameplayStartTime;

        public Bindable<double> UserPlaybackRate = new BindableDouble(1)
        {
            Default = 1,
            MinValue = 0.1,
            MaxValue = 2,
            Precision = 0.1,
        };

        /// <summary>The final clock which is exposed to underlying components.</summary>
        [Cached]
        public readonly GameplayClock GameplayClock;

        private readonly FramedOffsetClock userOffsetClock;

        private readonly FramedOffsetClock platformOffsetClock;

        public GameplayClockContainer(BeatmapMeta beatmap, double gameplayStartTime)
        {
            this.beatmap = beatmap;
            this.gameplayStartTime = gameplayStartTime;

            RelativeSizeAxes = Axes.Both;

            sourceClock = (IAdjustableClock)beatmap.Song ?? new StopwatchClock();

            adjustableClock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };

            platformOffsetClock = new FramedOffsetClock(adjustableClock) { Offset = RuntimeInfo.OS == RuntimeInfo.Platform.Windows ? 22 : 0 };

            // the final usable gameplay clock with user-set offsets applied.
            userOffsetClock = new FramedOffsetClock(platformOffsetClock);

            // the clock to be exposed via DI to children.
            GameplayClock = new GameplayClock(userOffsetClock);

            GameplayClock.IsPaused.BindTo(IsPaused);

            Seek(gameplayStartTime);
        }

        private double totalOffset => userOffsetClock.Offset + platformOffsetClock.Offset;

        public void Restart()
        {
            Task.Run(() =>
            {
                sourceClock.Reset();

                Schedule(() =>
                {
                    adjustableClock.ChangeSource(sourceClock);
                    updateRate();

                    if (!IsPaused.Value)
                        Start();
                });
            });
        }

        public void Start()
        {
            Seek(GameplayClock.CurrentTime);
            adjustableClock.Start();
            IsPaused.Value = false;
        }

        /// <summary>Seek to a specific time in gameplay.
        /// <remarks>Adjusts for any offsets which have been applied (so the seek may not be the expected point in time on the underlying audio track).</remarks>
        /// </summary>
        /// <param name="time">The destination time to seek to.</param>
        public void Seek(double time)
        {
            // remove the offset component here because most of the time we want the seek to be aligned to gameplay, not the audio track.
            // we may want to consider reversing the application of offsets in the future as it may feel more correct.
            adjustableClock.Seek(time - totalOffset);

            // manually process frame to ensure GameplayClock is correctly updated after a seek.
            userOffsetClock.ProcessFrame();
        }

        public void Stop()
        {
            adjustableClock.Stop();
            IsPaused.Value = true;
        }

        public void ResetLocalAdjustments()
        {
            // In the case of replays, we may have changed the playback rate.
            UserPlaybackRate.Value = 1;
        }

        protected override void Update()
        {
            if (!IsPaused.Value)
                userOffsetClock.ProcessFrame();

            base.Update();
        }

        private void updateRate()
        {
            if (sourceClock == null) return;

            sourceClock.ResetSpeedAdjustments();

            if (sourceClock is IHasTempoAdjust tempo)
                tempo.TempoAdjust = UserPlaybackRate.Value;
            else
                sourceClock.Rate = UserPlaybackRate.Value;
        }
    }
}
