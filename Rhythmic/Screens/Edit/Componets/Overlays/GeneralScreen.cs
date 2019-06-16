﻿using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;
using System.Linq;
using osu.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osuTK.Input;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.UserInterface;
using Rhythmic.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using Rhythmic.Screens.Edit.Componets.Overlays.LabelledComponents;

namespace Rhythmic.Screens.Edit.Componets.Overlays
{
    public class GeneralScreen : BeatmapMetadataScreen
    {
        private LabelledTextBox levelTitle;

        private LabelledTextBox romanisedLevelTitle;

        private LabelledTextBox difficulty;

        private LabelledTextBox source;

        private LabelledTextBox tags;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddInternal(new RhythmicScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Vertical,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(0, 10),
                        Children = new Drawable[]
                        {
                            levelTitle = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Level Title",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Level?.LevelName ?? ""
                            },
                            romanisedLevelTitle = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Romanised Level Title",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Level?.LevelNameUnicode ?? ""
                            },
                            new Box
                            {
                                Name = "Spacer",
                                RelativeSizeAxes = Axes.X,
                                Height = 1,
                                Colour = Color4.Transparent
                            },
                            difficulty = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Difficulty",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Level?.Difficulty ?? ""
                            },
                            source = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Source",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Level?.Source ?? ""
                            },
                            tags = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Tags"
                            },
                        }
                    }
                }
            });

            levelTitle.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Level.LevelName = val;
            romanisedLevelTitle.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Level.LevelNameUnicode = val;
            difficulty.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Level.Difficulty = val;
            source.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Level.Source = val;
            tags.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Level.Tags.Add(val);
        }
    }
}