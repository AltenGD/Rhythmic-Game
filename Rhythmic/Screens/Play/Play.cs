using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Colors;
using Rhythmic.Screens.MainMenu.Components;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Framework.Graphics.Containers;
using Rhythmic.Screens.Select;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Other;
using osuTK;
using Rhythmic.Screens.Edit;
using osu.Framework.Graphics.UserInterface;
using Rhythmic.Overlays;

namespace Rhythmic.Screens.Play
{
    public class Play : RhythmicScreen
    {
        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.Disabled;

        public override bool HideOverlaysOnEnter => true;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            collection.CurrentBeatmap.Value.Song.Stop();
        }
    }
}
