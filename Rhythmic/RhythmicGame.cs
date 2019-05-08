using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Graphics.Colors;
using Rhythmic.Other;
using Rhythmic.Overlays;
using Rhythmic.Overlays.Toolbar;
using Rhythmic.Screens;
using Rhythmic.Screens.MainMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Environment;

namespace Rhythmic
{
    public class RhythmicGame : RhythmicGameBase
    {
        public Toolbar Toolbar;

        private RhythmicScreenStack screenStack;

        private DependencyContainer dependencies;

        private MainMenu menuScreen;

        private MusicController musicController;

        private readonly List<OverlayContainer> overlays = new List<OverlayContainer>();

        private readonly List<OverlayContainer> visibleBlockingOverlays = new List<OverlayContainer>();

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public float ToolbarOffset => Toolbar.Position.Y + Toolbar.DrawHeight;

        /// <summary>Close all game-wide overlays.</summary>
        /// <param name="toolbar">Whether the toolbar should also be hidden.</param>
        public void CloseAllOverlays(bool toolbar = true)
        {
            foreach (var overlay in overlays)
                overlay.State = Visibility.Hidden;
            if (toolbar) Toolbar.State = Visibility.Hidden;
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

        public RhythmicGame(string[] args)
        { }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                screenContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        screenStack = new RhythmicScreenStack { RelativeSizeAxes = Axes.Both },
                    }
                },
                rightFloatingOverlayContent = new Container { RelativeSizeAxes = Axes.Both },
                topMostOverlayContent = new Container { RelativeSizeAxes = Axes.Both },
            });

            screenStack.Push(new Loader());

            loadComponentSingleFile(Toolbar = new Toolbar
            {
                OnHome = delegate
                {
                    CloseAllOverlays(false);
                    menuScreen?.MakeCurrent();
                },
            }, topMostOverlayContent.Add);

            loadComponentSingleFile(musicController = new MusicController
            {
                GetToolbarHeight = () => ToolbarOffset,
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
            }, rightFloatingOverlayContent.Add);

            Toolbar.ToggleVisibility();

            dependencies.Cache(musicController);
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
                var previousLoadStream = asyncLoadStream;

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
                        var del = new ScheduledDelegate(() => task = LoadComponentAsync(d, add));
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

        private Container topMostOverlayContent;

        private Container rightFloatingOverlayContent;

        private Container screenContainer;

        protected override void UpdateAfterChildren()
        {
            screenContainer.Padding = new MarginPadding { Top = ToolbarOffset };
            base.UpdateAfterChildren();
        }
    }
}
