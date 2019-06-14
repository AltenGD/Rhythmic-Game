using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Threading;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.UserInterface;
using Rhythmic.Overlays.Music;

namespace Rhythmic.Overlays
{
    public class MusicController : RhythmicFocusedOverlayContainer
    {
        private const float player_height = 130;
        private const float transition_length = 800;
        private const float progress_height = 10;
        private const float bottom_black_area_height = 55;

        private Drawable background;
        private ProgressBar progressBar;

        private IconButton prevButton;
        private IconButton playButton;
        private IconButton nextButton;
        private IconButton playlistButton;

        private SpriteText title, artist;

        private PlaylistOverlay playlist;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        [Resolved]
        private AudioManager audio { get; set; }

        private Container dragContainer;
        private Container playerContainer;

        private BufferedContainer screen;

        /// <summary>Provide a source for the toolbar height.</summary>
        public Func<float> GetToolbarHeight;

        public MusicController(BufferedContainer Screen)
        {
            Width = 400;
            Margin = new MarginPadding(10);

            AlwaysPresent = true;

            screen = Screen;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                dragContainer = new DragContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        playlist = new PlaylistOverlay(screen)
                        {
                            RelativeSizeAxes = Axes.X,
                            Y = player_height + 10,
                            OrderChanged = playlistOrderChanged
                        },
                        playerContainer = new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = player_height,
                            Masking = true,
                            CornerRadius = 5,
                            EdgeEffect = new EdgeEffectParameters
                            {
                                Colour = Color4.Black.Opacity(40),
                                Type = EdgeEffectType.Shadow,
                                Offset = new Vector2(0, 2),
                                Radius = 5,
                            },
                            Children = new[]
                            {
                                background = new Background(),
                                title = new SpriteText
                                {
                                    Origin = Anchor.BottomCentre,
                                    Anchor = Anchor.TopCentre,
                                    Position = new Vector2(0, 40),
                                    Font = RhythmicFont.GetFont(size: 35, italics: true),
                                    Colour = Color4.White,
                                    Text = @"Nothing to play",
                                },
                                artist = new SpriteText
                                {
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    Position = new Vector2(0, 45),
                                    Font = RhythmicFont.GetFont(size: 20, weight: FontWeight.Bold, italics: true),
                                    Colour = Color4.White,
                                    Text = @"Nothing to play",
                                },
                                new Container
                                {
                                    Padding = new MarginPadding { Bottom = progress_height },
                                    Height = bottom_black_area_height,
                                    RelativeSizeAxes = Axes.X,
                                    Origin = Anchor.BottomCentre,
                                    Anchor = Anchor.BottomCentre,
                                    Children = new Drawable[]
                                    {
                                        new FillFlowContainer<IconButton>
                                        {
                                            AutoSizeAxes = Axes.Both,
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(5),
                                            Origin = Anchor.Centre,
                                            Anchor = Anchor.Centre,
                                            Children = new[]
                                            {
                                                prevButton = new MusicIconButton
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Action = prev,
                                                    Icon = FontAwesome.Solid.StepBackward,
                                                },
                                                playButton = new MusicIconButton
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Scale = new Vector2(1.4f),
                                                    IconScale = new Vector2(1.4f),
                                                    Action = play,
                                                    Icon = FontAwesome.Regular.PlayCircle,
                                                },
                                                nextButton = new MusicIconButton
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Action = () => next(),
                                                    Icon = FontAwesome.Solid.StepForward,
                                                },
                                            }
                                        },
                                        playlistButton = new MusicIconButton
                                        {
                                            Origin = Anchor.Centre,
                                            Anchor = Anchor.CentreRight,
                                            Position = new Vector2(-bottom_black_area_height / 2, 0),
                                            Icon = FontAwesome.Solid.Bars,
                                            Action = () => playlist.ToggleVisibility(),
                                        },
                                    }
                                },
                                progressBar = new ProgressBar
                                {
                                    Origin = Anchor.BottomCentre,
                                    Anchor = Anchor.BottomCentre,
                                    Height = progress_height,
                                    FillColour = RhythmicColors.Orange,
                                    OnSeek = attemptSeek
                                }
                            },
                        },
                    }
                }
            };

            playlist.State.ValueChanged += s => playlistButton.FadeColour(s.NewValue == Visibility.Visible ? RhythmicColors.Orange : Color4.White, 200, Easing.OutQuint);
        }

        private ScheduledDelegate seekDelegate;

        private void attemptSeek(double progress)
        {
            seekDelegate?.Cancel();
            seekDelegate = Schedule(() =>
            {
                current?.Song.Seek(progress);
            });
        }

        private void playlistOrderChanged(BeatmapMeta beatmap, int index)
        {
            collection.Beatmaps.Remove(beatmap);
            collection.Beatmaps.Insert(index, beatmap);
        }

        protected override void LoadComplete()
        {
            collection.CurrentBeatmap.BindValueChanged(beatmapChanged, true);
            collection.CurrentBeatmap.BindDisabledChanged(beatmapDisabledChanged, true);
            base.LoadComplete();
        }

        private void beatmapDisabledChanged(bool disabled)
        {
            if (disabled)
                playlist.Hide();

            playButton.Enabled.Value = !disabled;
            prevButton.Enabled.Value = !disabled;
            nextButton.Enabled.Value = !disabled;
            playlistButton.Enabled.Value = !disabled;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();
            Height = dragContainer.Height;

            dragContainer.Padding = new MarginPadding { Top = GetToolbarHeight?.Invoke() ?? 0 };
        }

        protected override void Update()
        {
            base.Update();

            var track = current?.Song?.IsLoaded ?? false ? current?.Song : null;

            if (track?.IsDummyDevice == false)
            {
                progressBar.EndTime = track.Length;
                progressBar.CurrentTime = track.CurrentTime;

                playButton.Icon = track.IsRunning ? FontAwesome.Regular.PauseCircle : FontAwesome.Regular.PlayCircle;
            }
            else
            {
                progressBar.CurrentTime = 0;
                progressBar.EndTime = 1;

                playButton.Icon = FontAwesome.Regular.PlayCircle;
            }
        }

        private void play()
        {
            var track = current?.Song;

            if (track == null)
            {
                if (!collection.CurrentBeatmap.Disabled)
                    next(true);
                return;
            }

            if (track.IsRunning)
                track.Stop();
            else
                track.Start();
        }

        private void prev()
        {
            queuedDirection = TransformDirection.Prev;

            var playable = collection.Beatmaps.TakeWhile(i => i.ID != current.ID).LastOrDefault() ?? collection.Beatmaps.LastOrDefault();

            if (playable != null)
            {
                collection.CurrentBeatmap.Value.Song.Reset();
                collection.CurrentBeatmap.Value = playable;
                collection.CurrentBeatmap.Value.Song.Restart();
            }
        }

        private void next(bool instant = false)
        {
            if (!instant)
                queuedDirection = TransformDirection.Next;

            var playable = collection.Beatmaps.SkipWhile(i => i.ID != current.ID).Skip(1).FirstOrDefault() ?? collection.Beatmaps.FirstOrDefault();
            Console.WriteLine(playable);

            if (playable != null)
            {
                collection.CurrentBeatmap.Value.Song.Reset();
                collection.CurrentBeatmap.Value = playable;
                collection.CurrentBeatmap.Value.Song.Restart();
            }
        }

        private BeatmapMeta current;
        private TransformDirection? queuedDirection;

        private void beatmapChanged(ValueChangedEvent<BeatmapMeta> beatmap)
        {
            TransformDirection direction = TransformDirection.None;

            if (current != null)
            {
                bool audioEquals = beatmap.NewValue?.Song.Equals(current.Song) ?? false;

                if (audioEquals)
                    direction = TransformDirection.None;
                else if (queuedDirection.HasValue)
                {
                    direction = queuedDirection.Value;
                    queuedDirection = null;
                }
                else
                {
                    var last = collection.Beatmaps.TakeWhile(b => b.ID != current?.ID).Count();
                    var next = beatmap.NewValue == null ? -1 : collection.Beatmaps.TakeWhile(b => b.ID != beatmap.NewValue?.ID).Count();

                    direction = last > next ? TransformDirection.Prev : TransformDirection.Next;
                }

                current.Song.Completed -= currentTrackCompleted;
            }

            current = beatmap.NewValue;

            if (current != null)
                current.Song.Completed += currentTrackCompleted;

            progressBar.CurrentTime = 0;

            updateDisplay(current, direction);

            queuedDirection = null;
        }

        private void currentTrackCompleted() => Schedule(() =>
        {
            if (!current.Song.Looping && !collection.CurrentBeatmap.Disabled && collection.Beatmaps.Any())
                next();
        });

        private ScheduledDelegate pendingBeatmapSwitch;

        private void updateDisplay(BeatmapMeta beatmap, TransformDirection direction)
        {
            pendingBeatmapSwitch?.Cancel();

            pendingBeatmapSwitch = Schedule(delegate
            {
                if (beatmap == null)
                {
                    title.Text = @"Nothing to play!";
                    artist.Text = @"Nothing to play!";
                }
                else
                {
                    SongMetadata metadata = beatmap.Metadata.Song;
                    title.Text = new LocalisedString((metadata.NameUnicode, metadata.Name));
                    artist.Text = new LocalisedString((metadata.AuthorUnicode, metadata.Author));
                }

                LoadComponentAsync(new Background(beatmap) { Depth = float.MaxValue }, newBackground =>
                {
                    switch (direction)
                    {
                        case TransformDirection.Next:
                            newBackground.Position = new Vector2(400, 0);
                            newBackground.MoveToX(0, 500, Easing.OutCubic);
                            background.MoveToX(-400, 500, Easing.OutCubic);
                            break;

                        case TransformDirection.Prev:
                            newBackground.Position = new Vector2(-400, 0);
                            newBackground.MoveToX(0, 500, Easing.OutCubic);
                            background.MoveToX(400, 500, Easing.OutCubic);
                            break;
                    }

                    background.Expire();
                    background = newBackground;

                    playerContainer.Add(newBackground);
                });
            });
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.FadeIn(transition_length, Easing.OutQuint);
            dragContainer.ScaleTo(1, transition_length, Easing.OutElastic);
        }

        protected override void PopOut()
        {
            base.PopOut();

            playlist.Hide();

            this.FadeOut(transition_length, Easing.OutQuint);
            dragContainer.ScaleTo(0.9f, transition_length, Easing.OutQuint);
        }

        private enum TransformDirection
        {
            None,
            Next,
            Prev
        }

        private class MusicIconButton : IconButton
        {
            public MusicIconButton()
            {
                HoverColour = RhythmicColors.OrangeDark.Opacity(0.6f);
                FlashColour = RhythmicColors.Orange;
            }
        }

        private class Background : Container
        {
            private readonly Sprite sprite;
            private readonly BeatmapMeta beatmap;
            private Sprite noise;

            public Background(BeatmapMeta beatmap = null)
            {
                this.beatmap = beatmap;
                Depth = float.MaxValue;
                RelativeSizeAxes = Axes.Both;

                BufferedContainer container = new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Child = sprite = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = RhythmicColors.Gray(150),
                        FillMode = FillMode.Fill,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre
                    }
                };

                Children = new Drawable[]
                {
                    container,
                    new BufferedContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = bottom_black_area_height,
                        Origin = Anchor.BottomCentre,
                        Anchor = Anchor.BottomCentre,
                        BlurSigma = new Vector2(10),
                        Child = container.CreateView().With(d =>
                        {
                            d.RelativeSizeAxes = Axes.Both;
                            d.SynchronisedDrawQuad = true;
                        })
                    },
                    noise = new Sprite
                    {
                        Colour = Color4.Black.Opacity(0.05f),
                        Scale = new Vector2(2)
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = bottom_black_area_height,
                        Origin = Anchor.BottomCentre,
                        Anchor = Anchor.BottomCentre,
                        Colour = Color4.Black.Opacity(0.2f)
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(TextureStore textures)
            {
                sprite.Texture = beatmap?.Background ?? textures.Get(@"Backgrounds/bg4");
                noise.Texture = textures.Get("AcrylicNoise.png");
            }
        }

        private class DragContainer : Container
        {
            protected override bool OnDragStart(DragStartEvent e)
            {
                return true;
            }

            protected override bool OnDrag(DragEvent e)
            {
                Vector2 change = e.MousePosition - e.MouseDownPosition;

                // Diminish the drag distance as we go further to simulate "rubber band" feeling.
                change *= change.Length <= 0 ? 0 : (float)Math.Pow(change.Length, 0.7f) / change.Length;

                this.MoveTo(change);
                return true;
            }

            protected override bool OnDragEnd(DragEndEvent e)
            {
                this.MoveTo(Vector2.Zero, 800, Easing.OutElastic);
                return base.OnDragEnd(e);
            }
        }
    }
}
