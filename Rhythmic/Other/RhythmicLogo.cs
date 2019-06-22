using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using Rhythmic.Visualizers;
using Rhythmic.Screens.MainMenu;
using osu.Framework.Extensions.Color4Extensions;

namespace Rhythmic.Other
{
    public class RhythmicLogo : ClickableContainer
    {
        public Key triggerKey;

        private Container logoHoverContainer, logoBounceContainer;

        private readonly IntroSequence intro;

        public bool IsTracking { get; set; }

        public RhythmicLogo()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Rotation = 45;

            Size = new Vector2(290);

            Children = new Drawable[]
            {
                intro = new IntroSequence
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                logoHoverContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        logoBounceContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    BorderColour = Color4.White,
                                    BorderThickness = 10,
                                    Masking = true,
                                    Children = new Drawable[]
                                    {
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Alpha = 0,
                                            AlwaysPresent = true
                                        },
                                        new LinearVisualizer
                                        {
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            BarsAmount = 9,
                                            BarWidth = 31,
                                            Spacing = 16,
                                            Rotation = -45,
                                            ValueMultiplier = 3500,
                                            Smoothness = 260,
                                            Position = new Vector2(145, 0)
                                        },
                                        new LinearVisualizer
                                        {
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            BarsAmount = 9,
                                            BarWidth = 31,
                                            Spacing = 16,
                                            Rotation = -45,
                                            ValueMultiplier = 3500,
                                            IsReversed = true,
                                            Smoothness = 260,
                                            Position = new Vector2(145, 0)
                                        },
                                        new Triangle
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Size = new Vector2(137, 127),
                                            Rotation = 135,
                                            Colour = Color4.Black.Opacity(0.3f),
                                            Position = new Vector2(5)
                                        },
                                        new Triangle
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Size = new Vector2(137, 127),
                                            Rotation = 135,
                                        },
                                    }
                                },
                                new Container
                                {
                                    Size = new Vector2(190, 190),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    BorderColour = Color4.White,
                                    BorderThickness = 10,
                                    Masking = true,
                                    Alpha = 0,
                                    Rotation = 45,
                                    Children = new Drawable[]
                                    {
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            AlwaysPresent = true,
                                            Colour = Color4.Transparent
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Schedule a new extenral animation. Handled queueing and finishing previous animations in a sane way.
        /// </summary>
        /// <param name="action">The animation to be performed</param>
        /// <param name="waitForPrevious">If true, the new animation is delayed until all previous transforms finish. If false, existing transformed are cleared.</param>
        public void AppendAnimatingAction(Action action, bool waitForPrevious)
        {
            void runnableAction()
            {
                if (waitForPrevious)
                    this.DelayUntilTransformsFinished().Schedule(action);
                else
                {
                    ClearTransforms();
                    action();
                }
            }

            if (IsLoaded)
                runnableAction();
            else
                Schedule(runnableAction);
        }

        public void PlayIntro()
        {
            const double length = 3150;
            const double fade = 200;

            logoHoverContainer.FadeOut().Delay(length).FadeIn(fade);
            intro.Show();
            intro.Start(length);
            intro.Delay(length + fade).FadeOut();
        }
    }
}
