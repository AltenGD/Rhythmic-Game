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
    public class RhythmicLogo : Container
    {
        public Key triggerKey;

        private Triangle tri;
        private Container ring, pulse, logoHoverContainer, logoBounceContainer;
        private LinearVisualizer visualizer, visualizer2;

        private readonly IntroSequence intro;

        public bool IsTracking { get; set; }

        private readonly Box flashLayer;

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
                                ring = new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    BorderColour = Color4.White,
                                    BorderThickness = 10,
                                    Masking = true,
                                    Children = new Drawable[]
                                    {
                                        flashLayer = new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Blending = BlendingMode.Additive,
                                            Colour = Color4.White,
                                            Alpha = 0,
                                            AlwaysPresent = true
                                        },
                                        visualizer = new LinearVisualizer
                                        {
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            BarsAmount = 9,
                                            BarWidth = 31,
                                            Spacing = 16,
                                            Rotation = -45,
                                            ValueMultiplier = 2500,
                                            Smoothness = 260,
                                            Position = new Vector2(145, 0)
                                        },
                                        visualizer2 = new LinearVisualizer
                                        {
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            BarsAmount = 9,
                                            BarWidth = 31,
                                            Spacing = 16,
                                            Rotation = -45,
                                            ValueMultiplier = 2500,
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
                                        tri = new Triangle
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Size = new Vector2(137, 127),
                                            Rotation = 135,
                                        },
                                    }
                                },
                                pulse = new Container
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
