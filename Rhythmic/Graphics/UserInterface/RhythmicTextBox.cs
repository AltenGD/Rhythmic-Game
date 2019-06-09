using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using Rhythmic.Graphics.Containers;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Graphics.Colors;

namespace Rhythmic.Graphics.UserInterface
{
    public class RhythmicTextBox : TextBox, IKeyBindingHandler<GlobalAction>
    {
        protected override float LeftRightPadding => 10;

        protected override SpriteText CreatePlaceholder() => new SpriteText
        {
            Font = RhythmicFont.GetFont(italics: true),
            Colour = new Color4(180, 180, 180, 255),
            Margin = new MarginPadding { Left = 2 },
        };

        public RhythmicTextBox()
        {
            Height = 40;
            TextContainer.Height = 0.5f;
            CornerRadius = 5;
            LengthLimit = 1000;

            Current.DisabledChanged += disabled => { Alpha = disabled ? 0.3f : 1; };

            BackgroundUnfocused = Color4.Black.Opacity(0.5f);
            BackgroundFocused = RhythmicColors.Gray(0.3f).Opacity(0.5f);
            BackgroundCommit = BorderColour = RhythmicColors.Blue;
        }

        protected override void OnFocus(FocusEvent e)
        {
            BorderThickness = 3;
            base.OnFocus(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            BorderThickness = 0;

            base.OnFocusLost(e);
        }

        protected override Drawable GetDrawableCharacter(char c) => new SpriteText { Text = c.ToString(), Font = RhythmicFont.GetFont(size: CalculatedTextSize) };

        public virtual bool OnPressed(GlobalAction action)
        {
            if (action == GlobalAction.Exit)
            {
                KillFocus();
                return true;
            }

            return false;
        }

        public bool OnReleased(GlobalAction action) => false;
    }
}
