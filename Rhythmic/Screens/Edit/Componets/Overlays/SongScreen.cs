using osu.Framework.Allocation;
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
    public class SongScreen : BeatmapMetadataScreen
    {
        [Resolved]
        private BeatmapCollection collection { get; set; }

        private LabelledTextBox songTitle;
        private LabelledTextBox romanisedSongTitle;
        private LabelledTextBox author;
        private LabelledTextBox romanisedAuthor;

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
                            songTitle = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Song Title",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Song?.Name ?? "",
                            },
                            romanisedSongTitle = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Romanised Song Title",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Song?.NameUnicode ?? ""
                            },
                            new Box
                            {
                                Name = "Spacer",
                                RelativeSizeAxes = Axes.X,
                                Height = 1,
                                Colour = Color4.Transparent
                            },
                            author = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Author",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Song?.Author ?? ""
                            },
                            romanisedAuthor = new LabelledTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                LabelText = "Romanised Author",
                                Text = collection?.CurrentBeatmap?.Value?.Metadata?.Song?.AuthorUnicode ?? ""
                            }
                        }
                    }
                }
            });

            songTitle.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Song.Name = val;
            romanisedSongTitle.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Song.NameUnicode = val;
            author.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Song.Author = val;
            romanisedAuthor.OnCommit += val => collection.CurrentBeatmap.Value.Metadata.Song.AuthorUnicode = val;
        }
    }
}
