using osu.Framework;
using osu.Framework.Platform;
using System;

namespace Rhythmic.Test
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            using (DesktopGameHost host = Host.GetSuitableHost(@"Rhythmic", true))
                host.Run(new RhythmicTestBrowser());
        }
    }
}
