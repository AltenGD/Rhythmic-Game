using Rhythmic.Beatmap.Properties.Level.Object;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;
using osuTK;
using System.Linq;
using Rhythmic.Screens.Play;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Bindables;
using static osu.Framework.Graphics.UserInterface.CircularProgress;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;

namespace Rhythmic.Beatmap.Drawables
{
    public class DrawableBeatmapObject : CompositeDrawable
    {
        private Object obj;

        private Player player;

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
            var DelayTillExpire = 0f;

            drawable.Name = obj?.Name ?? "";
            drawable.Colour = new Color4(obj?.Colour[0] ?? 255, obj?.Colour[1] ?? 255, obj?.Colour[2] ?? 255, obj.Helper ? obj?.Colour[3] ?? 1 : 1);
            drawable.Size = new Vector2(obj?.Size[0] ?? 0, obj?.Size[1] ?? 0); 
            drawable.Anchor = obj?.Anchor ?? Anchor.TopLeft;
            drawable.Origin = obj?.Origin ?? Anchor.TopLeft;
            drawable.RelativeSizeAxes = obj?.RelativeSizeAxes ?? Axes.None;
            drawable.Shear = new Vector2(obj.Shear[0], obj.Shear[1]);

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
                        DelayTillExpire += (float)t.Time;
                        DelayTillExpire += (float)t.TimeUntilFinish;
                        container.Delay(t.Time).TransformTo(
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
                        DelayTillExpire += (float)t.Time;
                        DelayTillExpire += (float)t.TimeUntilFinish;
                        container.Delay(t.Time).TransformTo(
                            nameof(container.BorderThickness),
                            t.Value,
                            t.TimeUntilFinish,
                            t.EaseType);
                    });
                }

                foreach (var children in obj.Childrens)
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
                        DelayTillExpire += (float)t.Time;
                        DelayTillExpire += (float)t.TimeUntilFinish;

                        progress
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
                        DelayTillExpire += (float)t.Time;
                        DelayTillExpire += (float)t.TimeUntilFinish;

                        progress
                            .Delay(t.Time)
                            .TransformTo(nameof(progress.InnerRadius),
                                         t.Value,
                                         t.TimeUntilFinish,
                                         t.EaseType);
                    });
                }
            }

            if (obj.Autokill)
                Scheduler.AddDelayed(() => Expire(), DelayTillExpire);

            #region Transforms
            //Colour
            if (obj.ColourKeyframes != null && obj.ColourKeyframes.Any())
            {
                obj.ColourKeyframes.ForEach(t =>
                {
                    DelayTillExpire += (float)t.Time;
                    DelayTillExpire += (float)t.TimeUntilFinish;
                    drawable.Delay(t.Time).FadeColour(
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
                    DelayTillExpire += (float)t.Time;
                    DelayTillExpire += (float)t.TimeUntilFinish;
                    this.Delay(t.Time).MoveTo(
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
                    DelayTillExpire += (float)t.Time;
                    DelayTillExpire += (float)t.TimeUntilFinish;
                    drawable.Delay(t.Time).ResizeTo(
                        new Vector2((float)t.Value[0], (float)t.Value[1]),
                        t.TimeUntilFinish,
                        t.EaseType);

                    this.Delay(t.Time).ResizeTo(
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
                    DelayTillExpire += (float)t.Time;
                    DelayTillExpire += (float)t.TimeUntilFinish;
                    drawable.Delay(t.Time).RotateTo(
                        (float)t.Value,
                        t.TimeUntilFinish,
                        t.EaseType);
                });
            }
            #endregion
        }

        protected override void Update()
        {
            base.Update();

            if (player != null && drawable.ScreenSpaceDrawQuad.AABBFloat.IntersectsWith(player.player.ScreenSpaceDrawQuad.AABBFloat) && !obj.Helper && !obj.Empty)
            {
                player.TakeDamage();
            }
        }
    }
}
