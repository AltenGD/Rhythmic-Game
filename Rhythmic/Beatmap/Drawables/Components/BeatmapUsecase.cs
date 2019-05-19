using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using System;
using osuTK.Graphics;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Beatmap.Drawables.Components
{
    public class BeatmapUsecase : Container
    {
        private DatabasedBeatmap beatmap;
        private TextFlowContainer textContainer;

        public BeatmapUsecase(DatabasedBeatmap beatmap)
        {
            this.beatmap = beatmap;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new Container
                {
                    Padding = new MarginPadding(5),
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        textContainer = new TextFlowContainer
                        {
                            Direction = FillDirection.Horizontal
                        }
                    }
                }
            };

            textContainer.AddText(new SpriteText
            {
                Text = "mapped by ",
                Colour = Color4.Black
            });

            textContainer.AddText(new SpriteTextLink(null)
            {
                Text = "Alten"
            });
        }
    }
}
