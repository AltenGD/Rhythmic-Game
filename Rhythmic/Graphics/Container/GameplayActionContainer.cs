using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Rhythmic.Graphics.Container
{
    public class GameplayActionContainer : KeyBindingContainer<GameplayAction>
    {
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(InputKey.Down, GameplayAction.Key1),
            new KeyBinding(InputKey.Right, GameplayAction.Key2),
            new KeyBinding(InputKey.Up, GameplayAction.Key3),
            new KeyBinding(InputKey.Left, GameplayAction.Key4),
        };
    }

    public enum GameplayAction
    {
        Key1,
        Key2,
        Key3,
        Key4
    }
}
