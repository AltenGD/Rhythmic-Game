using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;
using osu.Framework.Timing;
using System;
using osu.Framework.MathUtils;
using osuTK;

namespace Rhythmic.Screens.Edit
{
    /// <summary>A decoupled clock which adds editor-specific functionality, such as snapping to a user-defined beat divisor.</summary>
    public class EditorClock : DecoupleableInterpolatingFramedClock
    {
        public readonly double TrackLength;

        private readonly BindableBeatDivisor beatDivisor;

        public EditorClock(BeatmapMeta beatmap, BindableBeatDivisor beatDivisor)
        {
            this.beatDivisor = beatDivisor;

            TrackLength = beatmap.Song.Length;
        }

        public EditorClock(double trackLength, BindableBeatDivisor beatDivisor)
        {
            this.beatDivisor = beatDivisor;

            TrackLength = trackLength;
        }

        /// <summary>Seeks backwards by one beat length.</summary>
        /// <param name="snapped">Whether to snap to the closest beat after seeking.</param>
        /// <param name="amount">The relative amount (magnitude) which should be seeked.</param>
        public void SeekBackward(double amount = -1) => seek(-1, amount);

        /// <summary>Seeks forwards by one beat length.</summary>
        /// <param name="snapped">Whether to snap to the closest beat after seeking.</param>
        /// <param name="amount">The relative amount (magnitude) which should be seeked.</param>
        public void SeekForward(double amount = 1) => seek(1, amount);

        private void seek(int direction, double amount = 1)
        {
            if (amount <= 0) throw new ArgumentException("Value should be greater than zero", nameof(amount));

            double seekAmount = beatDivisor.Value * amount;
            double seekTime = CurrentTime + seekAmount * direction * 500;

            Seek(seekTime);
            return;
        }
    }
}
