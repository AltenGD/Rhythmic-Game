using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Beatmap.Drawables
{
    public class BeatmapSetOnlineStatusPill : CircularContainer
    {
        private readonly SpriteText statusText;

        private BeatmapSetOnlineStatus status;

        public BeatmapSetOnlineStatus Status
        {
            get => status;
            set
            {
                if (status == value)
                    return;

                status = value;

                Alpha = value == BeatmapSetOnlineStatus.None ? 0 : 1;
                statusText.Text = value.ToString().ToUpperInvariant();
            }
        }

        public float TextSize
        {
            get => statusText.Font.Size;
            set => statusText.Font = statusText.Font.With(size: value);
        }

        public MarginPadding TextPadding
        {
            get => statusText.Padding;
            set => statusText.Padding = value;
        }

        public BeatmapSetOnlineStatusPill()
        {
            AutoSizeAxes = Axes.Both;
            Masking = true;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
                statusText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = RhythmicFont.GetFont(Typeface.Audiowide)
                },
            };

            Status = BeatmapSetOnlineStatus.None;
        }
    }
}
