﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using Rhythmic.Graphics.Sprites;

namespace Rhythmic.Beatmap.Drawables.Components
{
    public class BeatmapUsecase : Container
    {
        private TextFlowContainer textContainer;

        public BeatmapUsecase(DatabasedBeatmap beatmap)
        {
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