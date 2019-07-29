using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;
using Rhythmic.Other;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Screens.Edit;
using Rhythmic.Screens.MainMenu.Components;
using Rhythmic.Screens.Select;

namespace Rhythmic.Screens.MainMenu
{
    public class MainMenu : RhythmicScreen
    {
        private readonly RhythmicLogo logo;

        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenDefault();

        private Screen songSelect;

        public MainMenu(BufferedContainer Screen)
        {
            AddRangeInternal(new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Margin = new MarginPadding(50),
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Children = new Drawable[]
                            {
                                logo = new RhythmicLogo
                                {
                                    Scale = new Vector2(0.5f),
                                    Alpha = 0,
                                    Action = () => this.Push(new VisualizerScreen())
                                },
                            }
                        },
                        new ButtonSystem(Screen)
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Margin = new MarginPadding(50),
                            OnPlay = onSolo,
                            OnEditor = () => this.Push(new Editor())
                        }
                    }
                }
            });
        }

        protected override void LoadAsyncComplete()
        {
            base.LoadAsyncComplete();
            preloadSongSelect();
        }

        private void preloadSongSelect()
        {
            if (songSelect == null)
                LoadComponentAsync(songSelect = new PlaySongSelect());
        }

        public void LoadToSolo() => Schedule(onSolo);

        private void onSolo() => this.Push(consumeSongSelect());

        private Screen consumeSongSelect()
        {
            var s = songSelect;
            songSelect = null;
            return s;
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            (Background as BackgroundScreenDefault)?.Next();
            logo.MoveToX(25).MoveToX(0, 1000, Easing.OutExpo);
            logo.FadeIn(1000, Easing.OutExpo);
            preloadSongSelect();
        }

        public override void OnEntering(IScreen last)
        {
            logo.MoveToX(25).MoveToX(0, 1000, Easing.OutExpo);
            logo.FadeIn(1000, Easing.OutExpo);
            base.OnEntering(last);
        }

        public override bool OnExiting(IScreen next)
        {
            logo.MoveToX(25, 1000, Easing.InExpo);
            logo.FadeOut(1000, Easing.OutExpo);
            return base.OnExiting(next);
        }
    }
}
