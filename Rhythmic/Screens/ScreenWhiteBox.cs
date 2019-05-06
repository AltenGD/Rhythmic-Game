using System;
using System.Collections.Generic;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Screens
{
    public class ScreenWhiteBox : RhythmicScreen
    {
        private readonly BackButton popButton;

        private const double transition_time = 1000;

        protected virtual IEnumerable<Type> PossibleChildren => null;

        private readonly FillFlowContainer textContainer;
        private readonly Container boxContainer;

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            //only show the pop button if we are entered form another screen.
            if (last != null)
                popButton.Alpha = 1;

            Alpha = 0;
            textContainer.Position = new Vector2(DrawSize.X / 16, 0);

            boxContainer.ScaleTo(0.2f);
            boxContainer.RotateTo(-20);

            using (BeginDelayedSequence(300, true))
            {
                boxContainer.ScaleTo(1, transition_time, Easing.OutElastic);
                boxContainer.RotateTo(0, transition_time / 2, Easing.OutQuint);

                textContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
                this.FadeIn(transition_time, Easing.OutExpo);
            }
        }

        public override bool OnExiting(IScreen next)
        {
            textContainer.MoveTo(new Vector2(DrawSize.X / 16, 0), transition_time, Easing.OutExpo);
            this.FadeOut(transition_time, Easing.OutExpo);

            return base.OnExiting(next);
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            textContainer.MoveTo(new Vector2(-(DrawSize.X / 16), 0), transition_time, Easing.OutExpo);
            this.FadeOut(transition_time, Easing.OutExpo);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            textContainer.MoveTo(Vector2.Zero, transition_time, Easing.OutExpo);
            this.FadeIn(transition_time, Easing.OutExpo);
        }

        public ScreenWhiteBox()
        {
            InternalChildren = new Drawable[]
            {
                boxContainer = new Container
                {
                    Size = new Vector2(0.3f),
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 20,
                    Masking = true,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,

                            Colour = getColourFor(GetType()),
                            Alpha = 0.2f,
                            Blending = BlendingMode.Additive,
                        },
                        textContainer = new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                new SpriteIcon
                                {
                                    Icon = FontAwesome.Solid.UniversalAccess,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(50),
                                },
                                new SpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = GetType().Name,
                                    Colour = getColourFor(GetType()).Lighten(0.8f),
                                    Font = RhythmicFont.GetFont(size: 70),
                                },
                                new SpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "is not yet ready for use!",
                                    Font = RhythmicFont.GetFont(size: 30),
                                },
                                new SpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "please check back a bit later.",
                                    Font = RhythmicFont.GetFont(size: 22),
                                },
                            }
                        },
                    }
                },
                popButton = new BackButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Alpha = 0,
                    Action = this.Exit
                }
            };
        }

        private Color4 getColourFor(Type type)
        {
            int hash = type.Name.GetHashCode();
            byte r = (byte)MathHelper.Clamp(((hash & 0xFF0000) >> 16) * 0.8f, 20, 255);
            byte g = (byte)MathHelper.Clamp(((hash & 0x00FF00) >> 8) * 0.8f, 20, 255);
            byte b = (byte)MathHelper.Clamp((hash & 0x0000FF) * 0.8f, 20, 255);
            return new Color4(r, g, b, 255);
        }
    }
}
