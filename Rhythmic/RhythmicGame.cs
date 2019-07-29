using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Metadata;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Containers;
using Rhythmic.Overlays;
using Rhythmic.Overlays.Notifications;
using Rhythmic.Overlays.Toolbar;
using Rhythmic.Screens;
using Rhythmic.Screens.Edit;
using Rhythmic.Screens.Edit.Componets;
using Rhythmic.Screens.MainMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhythmic
{
    public class RhythmicGame : RhythmicGameBase, IKeyBindingHandler<GlobalAction>
    {
        public Toolbar Toolbar;

        private NotificationOverlay notifications;

        private RhythmicScreenStack screenStack;

        private DependencyContainer dependencies;

        private MainMenu menuScreen;

        private MusicController musicController;

        private BeatmapCollection beatmaps { get; set; }

        private readonly List<OverlayContainer> overlays = new List<OverlayContainer>();

        private readonly List<OverlayContainer> visibleBlockingOverlays = new List<OverlayContainer>();

        private readonly List<OverlayContainer> toolbarElements = new List<OverlayContainer>();

        public RhythmicGame(string[] args = null)
        {
            forwardLoggedErrorsToNotifications();

            if (beatmaps == null)
                beatmaps = new BeatmapCollection();

            if (beatmaps.Beatmaps == null)
                beatmaps.Beatmaps = new BindableList<BeatmapMeta>();

            if (beatmaps.CurrentBeatmap == null)
                beatmaps.CurrentBeatmap = new Bindable<BeatmapMeta>();

            Name = "RhythmicGame";
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public float ToolbarOffset => Toolbar.Position.Y + Toolbar.DrawHeight;

        public readonly Bindable<OverlayActivation> OverlayActivationMode = new Bindable<OverlayActivation>(OverlayActivation.All);

        private ScheduledDelegate performFromMainMenuTask;

        /// <summary>Perform an action only after returning to the main menu.
        /// Eagerly tries to exit the current screen until it succeeds.</summary>
        /// <param name="action">The action to perform once we are in the correct state.</param>
        /// <param name="taskName">The task name to display in a notification (if we can't immediately reach the main menu state).</param>
        /// <param name="targetScreen">An optional target screen type. If this screen is already current we can immediately perform the action without returning to the menu.</param>
        private void performFromMainMenu(Action action, string taskName, Type targetScreen = null, bool bypassScreenAllowChecks = false)
        {
            performFromMainMenuTask?.Cancel();

            // if the current screen does not allow screen changing, give the user an option to try again later.
            if (!bypassScreenAllowChecks && (screenStack.CurrentScreen as IRhythmicScreen)?.AllowExternalScreenChange == false)
            {
                notifications.Post(new SimpleNotification
                {
                    Text = $"Click here to {taskName}",
                    Activated = () =>
                    {
                        performFromMainMenu(action, taskName, targetScreen, true);
                        return true;
                    }
                });

                return;
            }

            CloseAllOverlays(false);

            // we may already be at the target screen type.
            if (targetScreen != null && screenStack.CurrentScreen?.GetType() == targetScreen)
            {
                action();
                return;
            }

            // all conditions have been met to continue with the action.
            if (menuScreen?.IsCurrentScreen() == true)
            {
                action();
                return;
            }

            // menuScreen may not be initialised yet (null check required).
            menuScreen?.MakeCurrent();

            performFromMainMenuTask = Schedule(() => performFromMainMenu(action, taskName));
        }

        /// <summary>Close all game-wide overlays.</summary>
        /// <param name="hideToolbarElements">Whether the toolbar (and accompanying controls) should also be hidden.</param>
        public void CloseAllOverlays(bool hideToolbarElements = true)
        {
            foreach (OverlayContainer overlay in overlays)
                overlay.State.Value = Visibility.Hidden;

            if (hideToolbarElements)
            {
                foreach (OverlayContainer overlay in toolbarElements)
                    overlay.State.Value = Visibility.Hidden;
            }
        }

        private void updateBlockingOverlayFade() =>
            screenContainer.FadeColour(visibleBlockingOverlays.Any() ? RhythmicColors.Gray(0.5f) : Color4.White, 500, Easing.OutQuint);

        public void AddBlockingOverlay(OverlayContainer overlay)
        {
            if (!visibleBlockingOverlays.Contains(overlay))
                visibleBlockingOverlays.Add(overlay);
            updateBlockingOverlayFade();
        }

        public void RemoveBlockingOverlay(OverlayContainer overlay)
        {
            visibleBlockingOverlays.Remove(overlay);
            updateBlockingOverlayFade();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            dependencies.Cache(this);

            AddRange(new Drawable[]
            {
                screenContainer = new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        screenStack = new RhythmicScreenStack { RelativeSizeAxes = Axes.Both },
                    }
                },
                rightFloatingOverlayContent = new BufferedContainer { RelativeSizeAxes = Axes.Both },
                topMostOverlayContent = new BufferedContainer { RelativeSizeAxes = Axes.Both },
            });

            screenStack.ScreenPushed += screenPushed;
            screenStack.ScreenExited += screenExited;

            dependencies.Cache(beatmaps);

            loadComponentSingleFile(Toolbar = new Toolbar(screenContainer)
            {
                OnHome = delegate
                {
                    CloseAllOverlays(false);
                    menuScreen?.MakeCurrent();
                },
            }, d =>
            {
                topMostOverlayContent.Add(d);
                toolbarElements.Add(d);
            });

            loadComponentSingleFile(musicController = new MusicController(screenContainer)
            {
                GetToolbarHeight = () => ToolbarOffset,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            }, m =>
            {
                rightFloatingOverlayContent.Add(m);
                overlays.Add(m);
            });

            loadComponentSingleFile(notifications = new NotificationOverlay(screenContainer)
            {
                GetToolbarHeight = () => ToolbarOffset,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            }, n =>
            {
                rightFloatingOverlayContent.Add(n);
                overlays.Add(n);
            });

            notifications.Post(new SimpleNotification
            {
                Text = "Test Notification",
                Description = "And here’s some text that you can almost barely read!"
            });

            Toolbar.ToggleVisibility();

            dependencies.Cache(menuScreen = new MainMenu(screenContainer));
            screenStack.Push(new Loader());

            dependencies.Cache(musicController);
            dependencies.Cache(notifications);

            OverlayActivationMode.ValueChanged += mode =>
            {
                if (mode.NewValue != OverlayActivation.All) CloseAllOverlays();
            };

            EditorBeatmapManager editorBeatmapManager;

            dependencies.Cache(editorBeatmapManager = new EditorBeatmapManager());

            Add(editorBeatmapManager);

            fileImporters.Add(editorBeatmapManager);

            editorBeatmapManager.PushToEditor += b =>
            {
                ProgressNotification notification = new ProgressNotification { State = ProgressNotificationState.Active };

                BeatmapMeta convertedBeatmap = Import(notification, b);

                if (convertedBeatmap != null)
                {
                    beatmaps.CurrentBeatmap.Value = convertedBeatmap;
                    screenStack.Push(new Editor());
                }
            };

            BeatmapMeta Import(ProgressNotification notification, string songPath)
            {
                notification.Progress = 0;
                notification.Text = "Import is initialising...";

                if (notification.State == ProgressNotificationState.Cancelled)
                    return null;

                try
                {
                    string text = "Importing " + Path.GetFileNameWithoutExtension(songPath);

                    notification.Text = text;

                    FileStream file = File.OpenRead(songPath);

                    BeatmapMeta beatmap = new BeatmapMeta
                    {
                        Song = new TrackBass(file),
                        SongUrl = songPath,
                        Metadata = new BeatmapMetadata
                        {
                            Song = new SongMetadata(),
                            Level = new LevelMetadata()
                        }
                    };

                    beatmaps.Beatmaps.Add(beatmap);

                    return beatmap;
                }
                catch (Exception e)
                {
                    e = e.InnerException ?? e;
                    Logger.Error(e, $@"Could not import ({Path.GetFileName(songPath)})");
                }

                return null;
            }
        }

        private void forwardLoggedErrorsToNotifications()
        {
            int recentLogCount = 0;

            const double debounce = 5000;

            Logger.NewEntry += entry =>
            {
                if (entry.Level < LogLevel.Important || entry.Target == null) return;

                const int short_term_display_limit = 3;

                if (recentLogCount < short_term_display_limit)
                {
                    Schedule(() => notifications.Post(new SimpleNotification
                    {
                        Icon = entry.Level == LogLevel.Important ? FontAwesome.Solid.ExclamationCircle : FontAwesome.Solid.Bomb,
                        Text = entry.Message + (entry.Exception != null && IsDeployedBuild ? "\n\nThis error has been automatically reported to the devs." : string.Empty),
                    }));
                }
                else if (recentLogCount == short_term_display_limit)
                {
                    Schedule(() => notifications.Post(new SimpleNotification
                    {
                        Icon = FontAwesome.Solid.EllipsisH,
                        Text = "Subsequent messages have been logged. Click to view log files.",
                        Activated = () =>
                        {
                            Host.Storage.GetStorageForDirectory("logs").OpenInNativeExplorer();
                            return true;
                        }
                    }));
                }

                Interlocked.Increment(ref recentLogCount);
                Scheduler.AddDelayed(() => Interlocked.Decrement(ref recentLogCount), debounce);
            };
        }

        private Task asyncLoadStream;

        private void loadComponentSingleFile<T>(T d, Action<T> add)
            where T : Drawable
        {
            // schedule is here to ensure that all component loads are done after LoadComplete is run (and thus all dependencies are cached).
            // with some better organisation of LoadComplete to do construction and dependency caching in one step, followed by calls to loadComponentSingleFile,
            // we could avoid the need for scheduling altogether.
            Schedule(() =>
            {
                Task previousLoadStream = asyncLoadStream;

                //chain with existing load stream
                asyncLoadStream = Task.Run(async () =>
                {
                    if (previousLoadStream != null)
                        await previousLoadStream;

                    try
                    {
                        Logger.Log($"Loading {d}...", level: LogLevel.Debug);

                        // Since this is running in a separate thread, it is possible for OsuGame to be disposed after LoadComponentAsync has been called
                        // throwing an exception. To avoid this, the call is scheduled on the update thread, which does not run if IsDisposed = true
                        Task task = null;
                        ScheduledDelegate del = new ScheduledDelegate(() => task = LoadComponentAsync(d, add));
                        Scheduler.Add(del);

                        // The delegate won't complete if OsuGame has been disposed in the meantime
                        while (!IsDisposed && !del.Completed)
                            await Task.Delay(10);

                        // Either we're disposed or the load process has started successfully
                        if (IsDisposed)
                            return;

                        Debug.Assert(task != null);

                        await task;

                        Logger.Log($"Loaded {d}!", level: LogLevel.Debug);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                });
            });
        }

        private BufferedContainer topMostOverlayContent;

        private BufferedContainer rightFloatingOverlayContent;

        private BufferedContainer screenContainer;

        private readonly bool IsDeployedBuild = true;

        protected override void UpdateAfterChildren()
        {
            screenContainer.Padding = new MarginPadding { Top = ToolbarOffset };
            base.UpdateAfterChildren();
        }

        protected virtual void ScreenChanged(IScreen current, IScreen newScreen)
        {
            if (newScreen is IRhythmicScreen newRhythmicScreen)
            {
                OverlayActivationMode.Value = newRhythmicScreen.InitialOverlayActivationMode;

                if (newRhythmicScreen.HideOverlaysOnEnter)
                    CloseAllOverlays();
                else
                    Toolbar.State.Value = Visibility.Visible;
            }
        }

        private void screenPushed(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen changed → {newScreen}");
        }

        private void screenExited(IScreen lastScreen, IScreen newScreen)
        {
            ScreenChanged(lastScreen, newScreen);
            Logger.Log($"Screen changed ← {newScreen}");

            if (newScreen == null)
                Exit();
        }

        public bool OnPressed(GlobalAction action)
        {
            switch (action)
            {
                case GlobalAction.ToggleToolbar:
                    Toolbar.ToggleVisibility();
                    return true;
            }

            return false;
        }

        public bool OnReleased(GlobalAction action) => false;
    }
}
