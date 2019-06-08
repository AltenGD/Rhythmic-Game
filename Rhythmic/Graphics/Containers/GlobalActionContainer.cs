using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

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
            new KeyBinding(new[] { InputKey.Control, InputKey.T }, GlobalAction.ToggleToolbar)
        };

        protected override IEnumerable<Drawable> KeyBindingInputQueue =>
            handler == null ? base.KeyBindingInputQueue : base.KeyBindingInputQueue.Prepend(handler);
    }

    public enum GlobalAction
    {
        Exit,
        ToggleToolbar
    }
}
