using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Overlays.Notifications
{
    public class NotificationSection : AlwaysUpdateFillFlowContainer<Drawable>
    {
        private SpriteText titleText;
        private SpriteText countText;

        private ClearAllButton clearButton;

        private FlowContainer<Notification> notifications;

        public int DisplayedCount => notifications.Count(n => !n.WasClosed);
        public int UnreadCount => notifications.Count(n => !n.WasClosed && !n.Read);

        public void Add(Notification notification, float position)
        {
            notifications.Add(notification);
            notifications.SetLayoutPosition(notification, position);
        }

        public IEnumerable<Type> AcceptTypes;

        private string clearText;

        public string ClearText
        {
            get => clearText;
            set
            {
                clearText = value;
                if (clearButton != null) clearButton.Text = clearText;
            }
        }

        private string title;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                if (titleText != null) titleText.Text = title.ToUpperInvariant();
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Vertical;

            Padding = new MarginPadding
            {
                Top = 10,
                Bottom = 5,
                Right = 20,
                Left = 20,
            };

            AddRangeInternal(new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        clearButton = new ClearAllButton
                        {
                            Text = clearText,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Action = clearAll
                        },
                        new FillFlowContainer
                        {
                            Margin = new MarginPadding
                            {
                                Bottom = 5
                            },
                            Spacing = new Vector2(5, 0),
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                titleText = new SpriteText
                                {
                                    Text = title.ToUpperInvariant(),
                                    Font = RhythmicFont.GetFont(size: 20, weight: FontWeight.Bold)
                                },
                                countText = new SpriteText
                                {
                                    Text = "3",
                                    Colour = RhythmicColors.Orange,
                                    Font = RhythmicFont.GetFont(size: 20, weight: FontWeight.Bold)
                                },
                            }
                        },
                    },
                },
                notifications = new AlwaysUpdateFillFlowContainer<Notification>
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    LayoutDuration = 150,
                    LayoutEasing = Easing.OutQuart,
                    Spacing = new Vector2(3),
                }
            });
        }

        private void clearAll()
        {
            notifications.Children.ForEach(c => c.Close());
        }

        protected override void Update()
        {
            base.Update();

            countText.Text = notifications.Children.Count(c => c.Alpha > 0.99f).ToString();
        }

        private class ClearAllButton : RhythmicClickableContainer
        {
            private readonly SpriteText text;

            public ClearAllButton()
            {
                AutoSizeAxes = Axes.Both;

                Children = new[]
                {
                    text = new SpriteText()
                };
            }

            public string Text
            {
                get => text.Text;
                set => text.Text = value.ToUpperInvariant();
            }
        }

        public void MarkAllRead()
        {
            notifications?.Children.ForEach(n => n.Read = true);
        }
    }

    public class AlwaysUpdateFillFlowContainer<T> : FillFlowContainer<T>
    where T : Drawable
    {
        protected override bool RequiresChildrenUpdate => true;
    }
}
