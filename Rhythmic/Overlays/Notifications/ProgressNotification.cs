using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Notifications
{
    public class ProgressNotification : Notification, IHasCompletionTarget
    {
        public string Text
        {
            set => Schedule(() => textDrawable.Text = value);
        }

        public string CompletionText { get; set; } = "Task has completed!";

        public float Progress
        {
            get => progressBar.Progress;
            set => Schedule(() => progressBar.Progress = value);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //we may have received changes before we were displayed.
            State = state;
        }

        public virtual ProgressNotificationState State
        {
            get => state;
            set =>
                Schedule(() =>
                {
                    bool stateChanged = state != value;
                    state = value;

                    if (IsLoaded)
                    {
                        switch (state)
                        {
                            case ProgressNotificationState.Queued:
                                progressBar.Active = false;
                                break;

                            case ProgressNotificationState.Active:
                                progressBar.Active = true;
                                break;

                            case ProgressNotificationState.Cancelled:
                                progressBar.Active = false;
                                break;
                        }
                    }

                    if (stateChanged)
                    {
                        switch (state)
                        {
                            case ProgressNotificationState.Completed:
                                NotificationContent.MoveToY(-DrawSize.Y / 2, 200, Easing.OutQuint);
                                this.FadeOut(200).Finally(d => Completed());
                                break;
                        }
                    }
                });
        }

        private ProgressNotificationState state;

        protected virtual Notification CreateCompletionNotification() => new ProgressCompletionNotification
        {
            Activated = CompletionClickAction,
            Text = CompletionText
        };

        protected virtual void Completed()
        {
            CompletionTarget?.Invoke(CreateCompletionNotification());
            base.Close();
        }

        public override bool DisplayOnTop => false;

        private readonly ProgressBar progressBar;

        private readonly TextFlowContainer textDrawable;

        public ProgressNotification()
        {
            IconContent.Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
            });

            Content.Add(textDrawable = new TextFlowContainer
            {
                Colour = RhythmicColors.Gray(128),
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
            });

            NotificationContent.Add(progressBar = new ProgressBar
            {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.BottomLeft,
                RelativeSizeAxes = Axes.X,
            });

            State = ProgressNotificationState.Queued;

            // don't close on click by default.
            Activated = () => false;
        }

        public override void Close()
        {
            switch (State)
            {
                case ProgressNotificationState.Cancelled:
                    base.Close();
                    break;

                case ProgressNotificationState.Active:
                case ProgressNotificationState.Queued:
                    if (CancelRequested?.Invoke() != false)
                        State = ProgressNotificationState.Cancelled;
                    break;
            }
        }

        public Func<bool> CancelRequested { get; set; }

        /// <summary>The function to post completion notifications back to.</summary>
        public Action<Notification> CompletionTarget { get; set; }

        /// <summary>An action to complete when the completion notification is clicked. Return true to close.</summary>
        public Func<bool> CompletionClickAction;

        private class ProgressBar : Container
        {
            private readonly Box box;

            private Color4 colourActive;
            private Color4 colourInactive;

            private float progress;

            public float Progress
            {
                get => progress;
                set
                {
                    if (progress == value) return;

                    progress = value;
                    box.ResizeTo(new Vector2(progress, 1), 100, Easing.OutQuad);
                }
            }

            private bool active;

            public bool Active
            {
                get => active;
                set
                {
                    active = value;
                    this.FadeColour(active ? colourActive : colourInactive, 100);
                }
            }

            public ProgressBar()
            {
                Children = new[]
                {
                    box = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Width = 0,
                    }
                };

                colourActive = RhythmicColors.Blue;
                Colour = colourInactive = RhythmicColors.Gray(0.5f);
                Height = 5;
            }
        }
    }

    public enum ProgressNotificationState
    {
        Queued,
        Active,
        Completed,
        Cancelled
    }
}
