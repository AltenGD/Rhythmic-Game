using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Rhythmic.Graphics.Containers
{
    public class GlobalActionContainer : KeyBindingContainer<GlobalAction>
    {
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(InputKey.Escape, GlobalAction.Exit)
        };
    }

    public enum GlobalAction
    {
        Exit
    }
}
