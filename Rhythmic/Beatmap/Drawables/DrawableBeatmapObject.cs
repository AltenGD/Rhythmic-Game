﻿using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;
using Rhythmic.Beatmap.Properties.Level.Object;
using Rhythmic.Screens.Play;
using System.Linq;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Beatmap.Drawables
{
    public class DrawableBeatmapObject : CompositeDrawable
    {
        private readonly Object obj;

        private readonly Player player;

        public DrawableBeatmapObject(Object obj, Player player = null)
        {
            this.obj = obj;
            this.player = player;
        }

        private Drawable drawable;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            drawable = this as Drawable;

            Size = new Vector2(obj?.Size[0] ?? 0, obj?.Size[1] ?? 0);
            Position = new Vector2(obj?.Position[0] ?? 0, obj?.Position[1] ?? 0);
            Anchor = obj?.Anchor ?? Anchor.TopLeft;
            Origin = obj?.Origin ?? Anchor.TopLeft;
            Blending = obj.BlendingParameters;

            switch (obj.Shape)
            {
                case Shape.Square:
                    CreateDrawable(drawable = new Box());
                    break;
                case Shape.Circle:
                    CreateDrawable(drawable = new Circle());
                    break;
                case Shape.Triangle:
                    CreateDrawable(drawable = new Triangle());
                    break;
                case Shape.EquilateralTriangle:
                    CreateDrawable(drawable = new EquilateralTriangle());
                    break;
                case Shape.Container:
                    CreateDrawable(drawable = new Container());
                    break;
                case Shape.CircularContainer:
                    CreateDrawable(drawable = new Container());
                    break;
                case Shape.CircularProgress:
                    CreateDrawable(drawable = new CircularProgress());
                    break;
                case Shape.Image:
                    break;
                default:
                    CreateDrawable(drawable = new Box());
                    break;
            }
        }

        private void CreateDrawable(Drawable drawable)
        {
            drawable.Name = obj?.Name ?? "";
            drawable.Colour = new Color4(obj?.Colour[0] ?? 255, obj?.Colour[1] ?? 255, obj?.Colour[2] ?? 255, obj.Helper ? obj?.Colour[3] ?? 1 : 1);
            drawable.Size = new Vector2(obj?.Size[0] ?? 0, obj?.Size[1] ?? 0);
            drawable.Anchor = obj?.Anchor ?? Anchor.TopLeft;
            drawable.Origin = obj?.Origin ?? Anchor.TopLeft;
            drawable.RelativeSizeAxes = obj?.RelativeSizeAxes ?? Axes.None;
            drawable.Shear = new Vector2(obj.Shear[0], obj.Shear[1]);

            
            this
                .FadeOut()
                .Then()
                .Delay(obj.Time)
                .Then()
                .FadeIn();

            AddInternal(drawable);

            if (drawable is Container container)
            {
                container.BorderColour = new Color4(obj?.BorderColour[0] ?? 255, obj?.BorderColour[1] ?? 255, obj?.BorderColour[2] ?? 255, obj?.BorderColour[3] ?? 255);
                container.BorderThickness = obj?.BorderThickness ?? 0f;
                container.Masking = obj?.Masking ?? false;
                container.AutoSizeAxes = obj?.AutoSizeAxes ?? Axes.None;

                //BorderColour
                if (obj.BorderColourKeyframes != null && obj.BorderColourKeyframes.Any())
                {
                    obj.BorderColourKeyframes.ForEach(t =>
                    {
                        container.Delay(obj.Time).Delay(t.Time).TransformTo(
                            nameof(container.BorderColour),
                            new Color4(t.Value[0], t.Value[1], t.Value[2], t.Value[3]),
                            t.TimeUntilFinish,
                            t.EaseType);
                    });
                }

                //BorderThickness
                if (obj.BorderThicknessKeyframes != null && obj.BorderThicknessKeyframes.Any())
                {
                    obj.BorderThicknessKeyframes.ForEach(t =>
                    {
                        container.Delay(obj.Time).Delay(t.Time).TransformTo(
                            nameof(container.BorderThickness),
                            t.Value,
                            t.TimeUntilFinish,
                            t.EaseType);
                    });
                }

                foreach (Object children in obj.Childrens)
                {
                    container.Add(new DrawableBeatmapObject(children));
                }
            }

            if (drawable is CircularProgress progress)
            {
                progress.Current = new Bindable<double>(obj.Fill);
                progress.InnerRadius = obj.InnerRadius;

                //Fill
                if (obj.FillKeyframes != null && obj.FillKeyframes.Any())
                {
                    obj.FillKeyframes.ForEach(t =>
                    {
                        progress
                            .Delay(obj.Time)
                            .Delay(t.Time)
                            .TransformTo(nameof(progress.Current.Value),
                                         t.Value,
                                         t.TimeUntilFinish,
                                         t.EaseType);
                    });
                }

                //InnerRadius
                if (obj.InnerRadiusKeyframes != null && obj.InnerRadiusKeyframes.Any())
                {
                    obj.InnerRadiusKeyframes.ForEach(t =>
                    {
                        progress
                            .Delay(obj.Time)
                            .Delay(t.Time)
                            .TransformTo(nameof(progress.InnerRadius),
                                         t.Value,
                                         t.TimeUntilFinish,
                                         t.EaseType);
                    });
                }
            }

            #region Transforms
            //Colour
            if (obj.ColourKeyframes != null && obj.ColourKeyframes.Any())
            {
                obj.ColourKeyframes.ForEach(t =>
                {
                    drawable.Delay(obj.Time).Delay(t.Time).FadeColour(
                        new Color4(t.Value[0], t.Value[1], t.Value[2], 255),
                        t.TimeUntilFinish,
                        t.EaseType)
                        .FadeTo(obj.Helper ? obj.Colour[3] / 2 : obj.Colour[3],
                            t.TimeUntilFinish,
                            t.EaseType);
                });
            }

            //Move
            if (obj.MoveKeyframes != null && obj.MoveKeyframes.Any())
            {
                obj.MoveKeyframes.ForEach(t =>
                {
                    this.Delay(obj.Time).Delay(t.Time).MoveTo(
                        new Vector2((float)t.Value[0], (float)t.Value[1]),
                        t.TimeUntilFinish,
                        t.EaseType);
                });
            }

            //Size
            if (obj.ScaleKeyframes != null && obj.ScaleKeyframes.Any())
            {
                obj.ScaleKeyframes.ForEach(t =>
                {
                    drawable.Delay(obj.Time).Delay(t.Time).ResizeTo(
                        new Vector2((float)t.Value[0], (float)t.Value[1]),
                        t.TimeUntilFinish,
                        t.EaseType);

                    this.Delay(obj.Time).Delay(t.Time).ResizeTo(
                        new Vector2((float)t.Value[0], (float)t.Value[1]),
                        t.TimeUntilFinish,
                        t.EaseType);
                });
            }

            //Rotation
            if (obj.RotationKeyframes != null && obj.RotationKeyframes.Any())
            {
                obj.RotationKeyframes.ForEach(t =>
                {
                    drawable.Delay(obj.Time).Delay(t.Time).RotateTo(
                        (float)t.Value,
                        t.TimeUntilFinish,
                        t.EaseType);
                });
            }
            #endregion

            if (obj.Autokill)
                Scheduler.AddDelayed(() => Expire(), obj.Time + obj.AbsoluteTotalTime);
        }

        protected override void Update()
        {
            base.Update();

            if (player != null &&
                drawable.ScreenSpaceDrawQuad.AABBFloat.IntersectsWith(
                    player.player.ScreenSpaceDrawQuad.AABBFloat) && !obj.Helper && !obj.Empty)
            {
                player.TakeDamage();
            }
        }
    }
}
