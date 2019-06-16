using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Overlays.Notifications
{
    public class SimpleNotification : Notification
    {
        private string text = string.Empty;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                textDrawable.Text = text;
            }
        }

        private string desc = string.Empty;

        public string Description
        {
            get => desc;
            set
            {
                desc = value;
                descDrawable.Text = desc;
            }
        }

        private IconUsage icon = FontAwesome.Solid.InfoCircle;

        public IconUsage Icon
        {
            get => icon;
            set
            {
                icon = value;
                iconDrawable.Icon = icon;
            }
        }

        public bool Important = false;

        public override bool IsImportant => Important;

        private readonly TextFlowContainer textDrawable;
        private readonly TextFlowContainer descDrawable;
        private readonly SpriteIcon iconDrawable;

        public Box IconBackgound;

        public SimpleNotification()
        {
            IconContent.AddRange(new Drawable[]
            {
                IconBackgound = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = ColourInfo.GradientVertical(RhythmicColors.BlueDark, RhythmicColors.BlueLight)
                },
                iconDrawable = new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Icon = icon,
                    Size = new Vector2(20),
                }
            });

            Content.Add(new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    textDrawable = new TextFlowContainer(t => t.Font = RhythmicFont.GetFont(size:20, weight:FontWeight.Bold))
                    {
                        Colour = Color4.White,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Text = text
                    },
                    descDrawable = new TextFlowContainer(t => t.Font = RhythmicFont.GetFont(size:15, weight:FontWeight.Bold))
                    {
                        Colour = Color4.White,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Text = desc
                    }
                }
            });
        }

        public override bool Read
        {
            get => base.Read;
            set
            {
                if (value == base.Read) return;

                base.Read = value;
            }
        }
    }
}
