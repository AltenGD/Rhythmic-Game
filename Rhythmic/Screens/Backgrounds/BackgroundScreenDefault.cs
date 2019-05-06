using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Framework.Threading;
using Rhythmic.Graphics;

namespace Rhythmic.Screens.Backgrounds
{
    public class BackgroundScreenDefault : BackgroundScreen
    {
        private Background background;

        private int currentDisplay;
        private const int background_count = 2;

        private string backgroundName => $@"Backgrounds/bg{currentDisplay % background_count + 1}";

        public BackgroundScreenDefault()
        {
            currentDisplay = RNG.Next(0, background_count);
            System.Console.WriteLine(backgroundName);

            display(createBackground());
        }

        private void display(Background newBackground)
        {
            background?.FadeOut(800, Easing.InOutSine);
            background?.Expire();

            AddInternal(background = newBackground);
            currentDisplay++;
        }

        private ScheduledDelegate nextTask;

        public void Next()
        {
            nextTask?.Cancel();
            nextTask = Scheduler.AddDelayed(() => { LoadComponentAsync(createBackground(), display); }, 100);
        }

        private Background createBackground()
        {
            Background newBackground;
            newBackground = new Background(backgroundName);
            newBackground.Depth = currentDisplay;

            return newBackground;
        }
    }
}
