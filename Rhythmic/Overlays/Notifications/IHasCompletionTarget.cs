using System;

namespace Rhythmic.Overlays.Notifications
{
    interface IHasCompletionTarget
    {
        Action<Notification> CompletionTarget { get; set; }
    }
}
