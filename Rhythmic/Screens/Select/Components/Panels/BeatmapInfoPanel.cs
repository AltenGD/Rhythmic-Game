using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Beatmap;
using System.Collections.Generic;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Linq;
using Rhythmic.Graphics.Sprites;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Screens.Select.Components.Panels
{
    public class BeatmapInfoPanel : InfoPanel
    {
        public override string Header => "beatmap info";

        public override float Opacity => 0.55f;

        public IEnumerable<Object> Objects;

        private double lastHitTime => Objects.LastOrDefault().Time + Objects.LastOrDefault().TotalTime + 1;

        private double firstHitTime => Objects.FirstOrDefault().Time;

        private double songLength => lastHitTime - firstHitTime;

        [BackgroundDependencyLoader]
        private void load(BeatmapCollection collection)
        {
            Console.WriteLine(songLength);

            AddRange(new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Margin = new MarginPadding
                            {
                                Right = 40
                            },
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = "Song Name: " + collection.CurrentBeatmap.Value.Metadata.Song.Name,
                                    Font = RhythmicFont.Default
                                },
                                new SpriteText
                                {
                                    Text = "Song Author: " + collection.CurrentBeatmap.Value.Metadata.Song.Author,
                                    Font = RhythmicFont.Default
                                },
                                new SpriteText
                                {
                                    Text = "",
                                    Font = RhythmicFont.Default
                                },
                                new SpriteText
                                {
                                    Text = "Beatmap Author: " + collection.CurrentBeatmap.Value.Metadata.Level.CreatorName,
                                    Font = RhythmicFont.Default
                                }
                            }
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = formatTime(TimeSpan.FromMilliseconds(songLength)),
                                    Font = RhythmicFont.Default,
                                    Anchor = Anchor.TopRight,
                                    Origin = Anchor.TopRight,
                                }
                            }
                        }
                    }
                }
            });
        }

        private string formatTime(TimeSpan timeSpan) 
            => $"{(timeSpan < TimeSpan.Zero ? "-" : "")}{Math.Floor(timeSpan.Duration().TotalMinutes)}:{timeSpan.Duration().Seconds:D2}";
    }
}
