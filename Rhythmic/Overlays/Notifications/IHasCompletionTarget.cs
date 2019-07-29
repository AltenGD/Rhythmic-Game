using System;

namespace Rhythmic.Overlays.Notifications
{
    internal interface IHasCompletionTarget
    {
        Action<Notification> CompletionTarget { get; set; }
    }
}
