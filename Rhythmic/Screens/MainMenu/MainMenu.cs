using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Screens.MainMenu.Components;
using System.IO;
using System.Text;
using static System.Environment;
using osu.Framework.Graphics.Containers;
using Rhythmic.Screens.Select;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Other;
using osuTK;
using Rhythmic.Screens.Edit;

namespace Rhythmic.Screens.MainMenu
{
    public class MainMenu : RhythmicScreen
    {
        private RhythmicLogo logo;

        public override bool AllowExternalScreenChange => true;

        protected override BackgroundScreen CreateBackground() => new BackgroundScreenDefault();

        public MainMenu()
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
                                    Alpha = 0
                                },
                            }
                        },
                        new ButtonSystem
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Margin = new MarginPadding(50),
                            OnPlay = () => this.Push(new SongSelect()),
                            OnEditor = () => this.Push(new Editor())
                        }
                    }
                }
            });
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            (Background as BackgroundScreenDefault)?.Next();
            logo.MoveToX(25).MoveToX(0, 1000, Easing.OutExpo);
            logo.FadeIn(1000, Easing.OutExpo);
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
