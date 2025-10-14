using TaleWorlds.InputSystem;

namespace DevKit.Hotkeys;

public class DevKitGameKeyContext : GameKeyContext
{
    public DevKitGameKeyContext()
        : base(nameof(DevKitGameKeyContext), 150) // Vanilla uses 108, just going higher to avoid conflicts
    {
        RegisterGameKey(
            new GameKey(
                (int)KeyMap.OpenWindowManager,
                "OpenWindowManager",
                nameof(DevKitGameKeyContext),
                InputKey.F9,
                nameof(DevKitGameKeyContext)
            )
        );
    }

    public enum KeyMap
    {
        OpenWindowManager = 145
    }
}
