using osu.Framework.Graphics;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Screens.MainMenu;
using System.IO;
using static System.Environment;

namespace Rhythmic
{
    public class RhythmicGame : RhythmicGameBase
    {
        public RhythmicGame(string[] args)
        { }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(new ScreenStack(new MainMenu
            {
                RelativeSizeAxes = Axes.Both
            }));

            var path = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\Beatmaps";
            var API = new BeatmapAPI();

            foreach (var file in Directory.EnumerateFiles(path))
            {
                int Length = path.Length;

                //beatmaps.Beatmaps.Add(API.GetBeatmapFromZip(file, file.Substring(Length + 1, file.Length - Length - 5)));
            }
        }
    }
}
