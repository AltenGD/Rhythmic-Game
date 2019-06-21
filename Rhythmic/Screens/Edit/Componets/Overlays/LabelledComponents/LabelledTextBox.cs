using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.UserInterface;
using System;

namespace Rhythmic.Screens.Edit.Componets.Overlays.LabelledComponents
{
    public class LabelledTextBox : CompositeDrawable
    {
        private const float label_container_width = 150;
        private const float corner_radius = 15;
        private const float default_height = 40;
        private const float default_label_left_padding = 15;
        private const float default_label_top_padding = 12;
        private const float default_label_text_size = 16;

        public event Action<string> OnCommit;

        public bool ReadOnly
        {
            get => textBox.ReadOnly;
            set => textBox.ReadOnly = value;
        }

        public string LabelText
        {
            get => label.Text;
            set => label.Text = value;
        }

        public float LabelTextSize
        {
            get => label.Font.Size;
            set => label.Font = label.Font.With(size: value);
        }

        public string PlaceholderText
        {
            get => textBox.PlaceholderText;
            set => textBox.PlaceholderText = value;
        }

        public string Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        public Color4 LabelTextColour
        {
            get => label.Colour;
            set => label.Colour = value;
        }

        private readonly RhythmicTextBox textBox;
        private readonly SpriteText label;

        public LabelledTextBox()
        {
            RelativeSizeAxes = Axes.X;
            Height = default_height;
            CornerRadius = corner_radius;
            Masking = true;

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = corner_radius,
                Masking = true,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black.Opacity(0.4f)
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = default_height,
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                label = new SpriteText
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Padding = new MarginPadding { Left = default_label_left_padding, Top = default_label_top_padding },
                                    Colour = Color4.White,
                                    Font = RhythmicFont.GetFont(size: default_label_text_size, weight: FontWeight.Bold),
                                },
                                textBox = new RhythmicTextBox
                                {
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    RelativeSizeAxes = Axes.Both,
                                    Height = 1,
                                    CornerRadius = corner_radius,
                                },
                            },
                        },
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, label_container_width),
                            new Dimension()
                        }
                    }
                }
            });

            textBox.OnCommit += (textBox, newText) => OnCommit.Invoke(textBox.Text);
        }
    }
}
