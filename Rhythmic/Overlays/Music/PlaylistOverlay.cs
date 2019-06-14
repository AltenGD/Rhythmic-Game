using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Overlays.Music
{
    public class PlaylistOverlay : OverlayContainer
    {
        private const float transition_duration = 600;
        private const float playlist_height = 510;

        /// <summary>Invoked when the order of an item in the list has changed.
        /// The second parameter indicates the new index of the item.</summary>
        public Action<BeatmapMeta, int> OrderChanged;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        private PlaylistList list;

        private BufferedContainer screen;

        public PlaylistOverlay(BufferedContainer Screen)
        {
            screen = Screen;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 5,
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = Color4.Black.Opacity(40),
                        Type = EdgeEffectType.Shadow,
                        Offset = new Vector2(0, 2),
                        Radius = 5,
                    },
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
                            Colour = Color4.Black.Opacity(0.25f),
                            RelativeSizeAxes = Axes.Both,
                        },
                        new Sprite
                        {
                            Texture = store.Get("AcrylicNoise.png"),
                            Colour = Color4.Black.Opacity(0.05f),
                            Scale = new Vector2(2)
                        },
                        list = new PlaylistList
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding { Top = 10, Bottom = 10, Right = 10 },
                            Selected = itemSelected,
                            OrderChanged = (s, i) => OrderChanged?.Invoke(s, i)
                        },
                    }
                }
            };
        }

        protected override void PopIn()
        {
            this.ResizeTo(new Vector2(1, playlist_height), transition_duration, Easing.OutQuint);
            this.FadeIn(transition_duration, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.ResizeTo(new Vector2(1, 0), transition_duration, Easing.OutQuint);
            this.FadeOut(transition_duration);
        }

        private void itemSelected(BeatmapMeta set)
        {
            if (set.ID == (collection.CurrentBeatmap.Value?.ID ?? -1))
            {
                collection.CurrentBeatmap.Value?.Song?.Seek(0);
                return;
            }

            collection.CurrentBeatmap.Value = collection.Beatmaps.First();
            collection.CurrentBeatmap.Value.Song.Restart();
        }
    }
}
