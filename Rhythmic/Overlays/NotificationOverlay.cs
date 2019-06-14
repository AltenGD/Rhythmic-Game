using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Threading;
using Rhythmic.Graphics.Containers;
using Rhythmic.Overlays.Notifications;
using osuTK;
using osu.Framework.Extensions.Color4Extensions;

namespace Rhythmic.Overlays
{
    public class NotificationOverlay : RhythmicFocusedOverlayContainer
    {
        private const float width = 320;

        public const float TRANSITION_LENGTH = 600;

        private BufferedContainer screen;

        private FlowContainer<NotificationSection> sections;

        /// <summary>Provide a source for the toolbar height.</summary>
        public Func<float> GetToolbarHeight;

        public NotificationOverlay(BufferedContainer Screen)
        {
            screen = Screen;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Width = width;
            RelativeSizeAxes = Axes.Y;

            AlwaysPresent = true;

            Children = new Drawable[]
            {
                new BufferedContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    BackgroundColour = Color4.Black,
                    BlurSigma = new Vector2(15),
                    Child = screen.CreateView().With(d =>
                    {
                        d.RelativeSizeAxes = Axes.Both;
                        d.SynchronisedDrawQuad = true;
                    })
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.2f)
                },
                content = new RhythmicScrollContainer
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        sections = new FillFlowContainer<NotificationSection>
                        {
                            Direction = FillDirection.Vertical,
                            AutoSizeAxes = Axes.Y,
                            RelativeSizeAxes = Axes.X,
                            Children = new[]
                            {
                                new NotificationSection
                                {
                                    Title = @"Notifications",
                                    ClearText = @"Clear All",
                                    AcceptTypes = new[] { typeof(SimpleNotification) }
                                },
                                new NotificationSection
                                {
                                    Title = @"Running Tasks",
                                    ClearText = @"Cancel All",
                                    AcceptTypes = new[] { typeof(ProgressNotification) }
                                }
                            }
                        }
                    }
                }
            };
        }

        private ScheduledDelegate notificationsEnabler;

        private void updateProcessingMode()
        {
            bool enabled = OverlayActivationMode.Value == OverlayActivation.All || State.Value == Visibility.Visible;

            notificationsEnabler?.Cancel();

            if (enabled)
                // we want a slight delay before toggling notifications on to avoid the user becoming overwhelmed.
                notificationsEnabler = Scheduler.AddDelayed(() => processingPosts = true, State.Value == Visibility.Visible ? 0 : 1000);
            else
                processingPosts = false;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            State.ValueChanged += _ => updateProcessingMode();
            OverlayActivationMode.BindValueChanged(_ => updateProcessingMode(), true);
        }

        private int totalCount => sections.Select(c => c.DisplayedCount).Sum();
        private int unreadCount => sections.Select(c => c.UnreadCount).Sum();

        public readonly BindableInt UnreadCount = new BindableInt();

        private int runningDepth;

        private void notificationClosed() => updateCounts();

        private readonly Scheduler postScheduler = new Scheduler();

        private bool processingPosts = true;
        private RhythmicScrollContainer content;

        public void Post(Notification notification) => postScheduler.Add(() =>
        {
            ++runningDepth;

            notification.Closed += notificationClosed;

            if (notification is IHasCompletionTarget hasCompletionTarget)
                hasCompletionTarget.CompletionTarget = Post;

            var ourType = notification.GetType();

            var section = sections.Children.FirstOrDefault(s => s.AcceptTypes.Any(accept => accept.IsAssignableFrom(ourType)));
            section?.Add(notification, notification.DisplayOnTop ? -runningDepth : runningDepth);

            if (notification.IsImportant)
                State.Value = Visibility.Visible;

            updateCounts();
        });

        protected override void Update()
        {
            base.Update();
            if (processingPosts)
                postScheduler.Update();
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.MoveToX(0, TRANSITION_LENGTH, Easing.OutQuint);
            content.FadeTo(1, TRANSITION_LENGTH, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            markAllRead();

            this.MoveToX(width, TRANSITION_LENGTH, Easing.OutQuint);
            content.FadeTo(0, TRANSITION_LENGTH, Easing.OutQuint);
        }

        private void updateCounts()
        {
            UnreadCount.Value = unreadCount;
        }

        private void markAllRead()
        {
            sections.Children.ForEach(s => s.MarkAllRead());

            updateCounts();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Padding = new MarginPadding { Top = GetToolbarHeight?.Invoke() ?? 0 };
        }
    }
}
