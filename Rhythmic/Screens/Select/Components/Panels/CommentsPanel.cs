using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Screens.Select.Components.Panels
{
    public class CommentsPanel : InfoPanel
    {
        public override string Header => "comments";

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRange(new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.Both,
                            Spacing = new Vector2(0, 5),
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = Header.ToUpper() + " Is not ready just yet!",
                                    Font = RhythmicFont.GetFont(size: 70),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre
                                },
                                new SpriteText
                                {
                                    Text = "It is still under development, so please come later once this has been finished!",
                                    Font = RhythmicFont.GetFont(size: 35),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    AllowMultiline = true
                                }
                            }
                        }
                    }
                },
            });
        }
    }
}
