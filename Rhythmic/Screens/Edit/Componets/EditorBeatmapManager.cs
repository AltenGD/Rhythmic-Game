using osu.Framework.Allocation;
using osu.Framework.Graphics;
using System;
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
