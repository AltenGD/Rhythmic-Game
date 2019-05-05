﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Platform;
using osuTK.Input;

namespace Rhythmic
{
    internal class RhythmicGameDesktop : RhythmicGame
    {
        public RhythmicGameDesktop(string[] args = null)
            : base(args) { }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //if (RuntimeInfo.OS == RuntimeInfo.Platform.Windows)
                //Add(new SquirrelUpdateManager());
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);
            if (host.Window is DesktopGameWindow desktopWindow)
            {
                desktopWindow.Title = "Rhythmic";

                desktopWindow.FileDrop += fileDrop;
            }
        }

        private void fileDrop(object sender, FileDropEventArgs e)
        {
            var filePaths = e.FileNames;

            var firstExtension = Path.GetExtension(filePaths.First());

            if (filePaths.Any(f => Path.GetExtension(f) != firstExtension)) return;

            Task.Factory.StartNew(() => Import(filePaths), TaskCreationOptions.LongRunning);
        }
    }
}
