using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using Rhythmic.Beatmap;
using Rhythmic.Other;
using Rhythmic.Screens;
using Rhythmic.Screens.MainMenu;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static System.Environment;

namespace Rhythmic
{
    public class RhythmicGame : RhythmicGameBase
    {
        private RhythmicScreenStack screenStack;

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public RhythmicGame(string[] args)
        { }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddRange(new Drawable[]
            {
                screenStack = new RhythmicScreenStack { RelativeSizeAxes = Axes.Both },
            });

            screenStack.Push(new Loader());

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
