using TaleWorlds.InputSystem;

namespace DevKit.Hotkeys;

public class DevKitGameKeyContext : GameKeyContext
{
    public DevKitGameKeyContext()
        : base(nameof(DevKitGameKeyContext), 150) // Vanilla uses 108, just going higher to avoid conflicts
    {
        RegisterGameKey(
            new GameKey(
                (int)KeyMap.OpenControlPanel,
                "OpenControlPanel",
                nameof(DevKitGameKeyContext),
                InputKey.F1,
                nameof(DevKitGameKeyContext)
            )
        );
        RegisterGameKey(
            new GameKey(
                (int)KeyMap.OpenMobilePartyDebugger,
                "OpenMobilePartyDebugger",
                nameof(DevKitGameKeyContext),
                InputKey.F2,
                nameof(DevKitGameKeyContext)
            )
        );
        RegisterGameKey(
            new GameKey(
                (int)KeyMap.OpenCampaignEventsDebugger,
                "OpenCampaignEventsDebugger",
                nameof(DevKitGameKeyContext),
                InputKey.F3,
                nameof(DevKitGameKeyContext)
            )
        );
        RegisterGameKey(
            new GameKey(
                (int)KeyMap.OpenMissionDebugger,
                "OpenMissionDebugger",
                nameof(DevKitGameKeyContext),
                InputKey.F4,
                nameof(DevKitGameKeyContext)
            )
        );
    }

    public enum KeyMap
    {
        OpenControlPanel = 140,
        OpenMobilePartyDebugger = 141,
        OpenCampaignEventsDebugger = 142,
        OpenMissionDebugger = 143,
    }
}
