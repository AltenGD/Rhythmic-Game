using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;
using System;

namespace Rhythmic.Screens.Play
{
    public class SongProgressInfo : Container
    {
        private SpriteText timeCurrent;
        private SpriteText timeLeft;
        private SpriteText progress;

        private double startTime;
        private double endTime;

        private int? previousPercent;
        private int? previousSecond;

        private double songLength => endTime - startTime;

        private const int margin = 5;

        public double StartTime
        {
            set => startTime = value;
        }

        public double EndTime
        {
            set => endTime = value;
        }

        private GameplayClock gameplayClock;

        [BackgroundDependencyLoader(true)]
        private void load(GameplayClock clock)
        {
            if (clock != null)
                gameplayClock = clock;

            Children = new Drawable[]
            {
                timeCurrent = new SpriteText
                {
                    Origin = Anchor.BottomLeft,
                    Anchor = Anchor.BottomLeft,
                    Colour = RhythmicColors.Blue,
                    Font = RhythmicFont.Numeric,
                    Margin = new MarginPadding
                    {
                        Left = margin,
                    },
                    Blending = new BlendingParameters
                    {
                        Mode = BlendingMode.Additive
                    }
                },
                progress = new SpriteText
                {
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    Colour = RhythmicColors.Blue,
                    Font = RhythmicFont.Numeric,
                    Blending = new BlendingParameters
                    {
                        Mode = BlendingMode.Additive
                    }
                },
                timeLeft = new SpriteText
                {
                    Origin = Anchor.BottomRight,
                    Anchor = Anchor.BottomRight,
                    Colour = RhythmicColors.Blue,
                    Font = RhythmicFont.Numeric,
                    Margin = new MarginPadding
                    {
                        Right = margin,
                    },
                    Blending = new BlendingParameters
                    {
                        Mode = BlendingMode.Additive
                    }
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            var time = gameplayClock?.CurrentTime ?? Time.Current;

            double songCurrentTime = time - startTime;
            int currentPercent = Math.Max(0, Math.Min(100, (int)(songCurrentTime / songLength * 100)));
            int currentSecond = (int)Math.Floor(songCurrentTime / 1000.0);

            if (currentPercent != previousPercent)
            {
                progress.Text = currentPercent.ToString() + @"%";
                previousPercent = currentPercent;
            }

            if (currentSecond != previousSecond && songCurrentTime < songLength)
            {
                timeCurrent.Text = formatTime(TimeSpan.FromSeconds(currentSecond));
                timeLeft.Text = formatTime(TimeSpan.FromMilliseconds(endTime - time));

                previousSecond = currentSecond;
            }
        }

        private string formatTime(TimeSpan timeSpan) => $"{(timeSpan < TimeSpan.Zero ? "-" : "")}{Math.Floor(timeSpan.Duration().TotalMinutes)}:{timeSpan.Duration().Seconds:D2}";
    }
}
