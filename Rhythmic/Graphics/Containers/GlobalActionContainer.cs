using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using System.Collections.Generic;
using System.Linq;

namespace Rhythmic.Graphics.Containers
{
    public class GlobalActionContainer : KeyBindingContainer<GlobalAction>
    {
        private readonly Drawable handler;

        public GlobalActionContainer(RhythmicGameBase game)
        {
            if (game is IKeyBindingHandler<GlobalAction>)
                handler = game;
        }

        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(InputKey.Escape, GlobalAction.Exit),
            new KeyBinding(new[] { InputKey.Control, InputKey.T }, GlobalAction.ToggleToolbar),
            new KeyBinding(InputKey.Space, GlobalAction.Select),
            new KeyBinding(InputKey.Enter, GlobalAction.Select),
            new KeyBinding(InputKey.KeypadEnter, GlobalAction.Select),
        };

        protected override IEnumerable<Drawable> KeyBindingInputQueue =>
            handler == null ? base.KeyBindingInputQueue : base.KeyBindingInputQueue.Prepend(handler);
    }

    public enum GlobalAction
    {
        Exit,
        ToggleToolbar,
        Select,
    }
}
