﻿using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osuTK;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Level;
using Rhythmic.Beatmap.Properties.Level.Keyframe;
using Rhythmic.Beatmap.Properties.Level.Object;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Containers;
using System.Collections.Generic;

namespace Rhythmic.Test.Visual.Beatmaps
{
    public class TestSceneBeatmapParsing : TestScene
    {
        private readonly FillFlowContainer Container;
        private readonly BeatmapMeta TestLevel;
        private readonly BeatmapAPI API;
        private TrackBass Track;

        private int index = 1;

        public TestSceneBeatmapParsing()
        {
            API = new BeatmapAPI();

            Children = new Drawable[]
            {
                new RhythmicScrollContainer(Direction.Vertical)
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        Container = new FillFlowContainer
                        {
                            LayoutDuration = 100,
                            LayoutEasing = Easing.Out,
                            Spacing = new Vector2(1, 1),
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Padding = new MarginPadding(5)
                        }
                    }
                }
            };

            #region BeatmapSetup
            TestLevel = new BeatmapMeta
            {
                Metadata = new BeatmapMetadata
                {
                    Song = new SongMetadata
                    {
                        Author = "Silentroom",
                        Name = "NULCTRL"
                    },
                    Level = new LevelMetadata
                    {
                        CreatorID = 0,
                        CreatorName = "Alten",
                        LevelName = "NULCTRL"
                    }
                },
                SongUrl = "song.mp3"
            };

            TestLevel.Player = new PlayerMeta
            {
                Colour = new float[] { 255, 255, 255, 255 },
                Cooldown = 2,
                Health = 3,
                Size = 20
            };

            TestLevel = API.CreateNewLevel(TestLevel);

            TestLevel.Level.BackgroundColour = new float[] { 10, 10, 10, 10 };

            API.AddObject(new Object
            {
                Name = "Object 2",
                Time = 2,
                Shape = Shape.Square,
                Position = new float[] { 0, 0 },
                Size = new float[] { 1, 1 },
                Colour = new float[] { 255, 255, 255, 255 },
                Rotation = 0,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Autokill = false,
                Masking = false,
                Empty = false,
                Helper = false,
                MoveKeyframes = new List<Keyframe<double[]>>
                {
                    new Keyframe<double[]>
                    {
                        Time = 0,
                        Value = new double[] { 0, 0 },
                        EaseType = Easing.None
                    },
                    new Keyframe<double[]>
                    {
                        Time = 1,
                        Value = new double[] { 0, 20 },
                        EaseType = Easing.InExpo
                    }
                },
                ScaleKeyframes = new List<Keyframe<double[]>>
                {
                    new Keyframe<double[]>
                    {
                        Time = 0,
                        Value = new double[] { 0, 0 },
                        EaseType = Easing.None
                    },
                    new Keyframe<double[]>
                    {
                        Time = 0.2,
                        Value = new double[] { 1, 1 },
                        EaseType = Easing.OutExpo
                    }
                }
            }, TestLevel);

            #endregion

            string Json = API.ParseBeatmap(TestLevel);

            System.Console.WriteLine(Json);

            Container.Add(new TextFlowContainer
            {
                Text = Json,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            });

            AddStep("aa", null);

            //TODO: Add more steps
            #region Steps
            AddStep("Create new object", () =>
            {
                API.AddObject(new Object
                {
                    Name = "Object " + index,
                    Time = 2,
                    Shape = Shape.Square,
                    Position = new float[] { 0, 0 },
                    Size = new float[] { 1, 1 },
                    Rotation = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Autokill = false,
                    Masking = false,
                    Empty = false,
                    Helper = false,
                    MoveKeyframes = new List<Keyframe<double[]>>
                    {
                        new Keyframe<double[]>
                        {
                            Time = 0,
                            Value = new double[] { 0, 0 },
                            EaseType = Easing.None
                        },
                        new Keyframe<double[]>
                        {
                            Time = 1,
                            Value = new double[] { 0, 20 },
                            EaseType = Easing.InExpo
                        }
                    },
                    ScaleKeyframes = new List<Keyframe<double[]>>
                    {
                        new Keyframe<double[]>
                        {
                            Time = 0,
                            Value = new double[] { 0, 0 },
                            EaseType = Easing.None
                        },
                        new Keyframe<double[]>
                        {
                            Time = 0.2,
                            Value = new double[] { 1, 1 },
                            EaseType = Easing.OutExpo
                        }
                    }
                }, TestLevel);

                Container.Clear();

                Json = JsonConvert.SerializeObject(TestLevel, Formatting.Indented);

                Container.Add(new TextFlowContainer
                {
                    Text = Json,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                });

                index++;
            });
            #endregion
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, Game game)
        {
            AddStep("Play track", () =>
            {
                Track = new TrackBass(game.Resources.GetStream("Tracks/" + TestLevel.Metadata.Level.LevelName + "/" + TestLevel.SongUrl));

                audio.AddItem(Track);
                Track.Start();
            });

            AddStep("Stop track", () =>
            {
                Track.Stop();
                Track.Seek(0);
            });
        }
    }
}
