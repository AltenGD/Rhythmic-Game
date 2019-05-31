using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;
using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics.Lines;
using osu.Framework.MathUtils;
using Rhythmic.Graphics.Drawables;
using Rhythmic.Graphics.Colors;
using System.Linq;
using osu.Framework;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osuTK.Input;
using osu.Framework.Extensions.Color4Extensions;
using Rhythmic.Graphics.Sprites;
using Object = Rhythmic.Beatmap.Properties.Level.Object.Object;
using Rhythmic.Database;
using Rhythmic.Overlays;
using Rhythmic.Overlays.Notifications;

namespace Rhythmic.Screens.Edit.Componets
{
    public class EditorBeatmapManager : Component, ICanAcceptFiles
    {
        private NotificationOverlay notification;

        public event Action<string> PushToEditor;

        public string[] HandledExtensions => new string[] { ".mp3", ".ogg", ".png", ".jpg", ".jpeg" };

        [BackgroundDependencyLoader(true)]
        private void load(NotificationOverlay notification)
        {
            this.notification = notification;
        }

        public void Import(params string[] paths)
        {
            notification.Post(new SimpleNotification
            {
                Text = @"Would you like to import """ + System.IO.Path.GetFileNameWithoutExtension(paths[0]) + @"""",
                Activated = ImportAsBeatmap
            });

            bool ImportAsBeatmap()
            {
                PushToEditor?.Invoke(paths[0]);

                return true;
            }
        }
    }
}
