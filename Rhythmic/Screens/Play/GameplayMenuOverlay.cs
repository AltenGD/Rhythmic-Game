using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osuTK.Input;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using Humanizer;
using osu.Framework.Graphics.Effects;
using Rhythmic.Graphics.Sprites;
using osu.Framework.Graphics.Sprites;
using Rhythmic.Graphics.Colors;
using Rhythmic.Graphics.UserInterface;

namespace Rhythmic.Screens.Play
{
    public abstract class GameplayMenuOverlay : OverlayContainer
    {
        private const int transition_duration = 200;
        private const int button_height = 70;
        private const float background_alpha = 0.75f;

        protected override bool BlockNonPositionalInput => true;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        public Action OnRetry;
        public Action OnQuit;

        /// <summary> Action that is invoked when <see cref="GlobalAction.Back"/> is triggered.</summary>
        protected virtual Action BackAction => () => InternalButtons.Children.LastOrDefault()?.Click();

        /// <summary>Action that is invoked when <see cref="GlobalAction.Select"/> is triggered.</summary>
        protected virtual Action SelectAction => () => InternalButtons.Children.FirstOrDefault(f => f.Selected.Value)?.Click();

        public abstract string Header { get; }

        public abstract string Description { get; }

        protected internal FillFlowContainer<DialogButton> InternalButtons;
        public IReadOnlyList<DialogButton> Buttons => InternalButtons;

        private FillFlowContainer retryCounterContainer;

        protected GameplayMenuOverlay()
        {
            RelativeSizeAxes = Axes.Both;

            StateChanged += s => selectionIndex = -1;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = background_alpha,
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 50),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 20),
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = Header,
                                    Font = RhythmicFont.GetFont(size: 30),
                                    Spacing = new Vector2(5, 0),
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    Colour = RhythmicColors.Orange,
                                    Shadow = true,
                                    ShadowColour = new Color4(0, 0, 0, 0.25f)
                                },
                                new SpriteText
                                {
                                    Text = Description,
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    Shadow = true,
                                    ShadowColour = new Color4(0, 0, 0, 0.25f)
                                }
                            }
                        },
                        InternalButtons = new FillFlowContainer<DialogButton>
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Masking = true,
                            EdgeEffect = new EdgeEffectParameters
                            {
                                Type = EdgeEffectType.Shadow,
                                Colour = Color4.Black.Opacity(0.6f),
                                Radius = 50
                            },
                        },
                        retryCounterContainer = new FillFlowContainer
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            AutoSizeAxes = Axes.Both,
                        }
                    }
                },
            };

            updateRetryCount();
        }

        private int retries;

        public int Retries
        {
            get => retries;
            set
            {
                if (value == retries)
                    return;

                retries = value;
                if (retryCounterContainer != null)
                    updateRetryCount();
            }
        }

        protected override void PopIn() => this.FadeIn(transition_duration, Easing.In);
        protected override void PopOut() => this.FadeOut(transition_duration, Easing.In);

        // Don't let mouse down events through the overlay or people can click circles while paused.
        protected override bool OnMouseDown(MouseDownEvent e) => true;

        protected override bool OnMouseUp(MouseUpEvent e) => true;

        protected override bool OnMouseMove(MouseMoveEvent e) => true;

        protected void AddButton(string text, Color4 colour, Action action)
        {
            var button = new Button
            {
                Text = text,
                ButtonColour = colour,
                Origin = Anchor.TopCentre,
                Anchor = Anchor.TopCentre,
                Height = button_height,
                Action = delegate
                {
                    action?.Invoke();
                    Hide();
                }
            };

            button.Selected.ValueChanged += selected => buttonSelectionChanged(button, selected.NewValue);

            InternalButtons.Add(button);
        }

        private int _selectionIndex = -1;

        private int selectionIndex
        {
            get => _selectionIndex;
            set
            {
                if (_selectionIndex == value)
                    return;

                // Deselect the previously-selected button
                if (_selectionIndex != -1)
                    InternalButtons[_selectionIndex].Selected.Value = false;

                _selectionIndex = value;

                // Select the newly-selected button
                if (_selectionIndex != -1)
                    InternalButtons[_selectionIndex].Selected.Value = true;
            }
        }

        private void buttonSelectionChanged(DialogButton button, bool isSelected)
        {
            if (!isSelected)
                selectionIndex = -1;
            else
                selectionIndex = InternalButtons.IndexOf(button);
        }

        private void updateRetryCount()
        {
            // "You've retried 1,065 times in this session"
            // "You've retried 1 time in this session"

            retryCounterContainer.Children = new Drawable[]
            {
                new SpriteText
                {
                    Text = "You've retried ",
                    Shadow = true,
                    ShadowColour = new Color4(0, 0, 0, 0.25f),
                    Font = RhythmicFont.GetFont(size: 18),
                },
                new SpriteText
                {
                    Text = "time".ToQuantity(retries),
                    Font = RhythmicFont.GetFont(weight: FontWeight.Bold, size: 18),
                    Shadow = true,
                    ShadowColour = new Color4(0, 0, 0, 0.25f),
                },
                new SpriteText
                {
                    Text = " in this session",
                    Shadow = true,
                    ShadowColour = new Color4(0, 0, 0, 0.25f),
                    Font = RhythmicFont.GetFont(size: 18),
                }
            };
        }

        private class Button : DialogButton
        {
            protected override bool OnHover(HoverEvent e) => true;

            protected override bool OnMouseMove(MouseMoveEvent e)
            {
                Selected.Value = true;
                return base.OnMouseMove(e);
            }
        }
    }
}
