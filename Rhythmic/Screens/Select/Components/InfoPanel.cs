using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Screens.Select.Components
{
    public class InfoPanel : Container
    {
        public virtual string Header => "";
        public virtual float Opacity => 0.4f;

        private Container content;

        protected override Container<Drawable> Content => content;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(Opacity)
                },
                new SpriteText
                {
                    Margin = new MarginPadding(5),
                    Font = RhythmicFont.Default,
                    Text = Header.ToUpper(),
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Horizontal = 15,
                        Bottom = 15,
                        Top = 55,
                    }
                }
            });
        }
    }
}
