using DevKit.Debuggers;
using DevKit.Hotkeys;
using HarmonyLib;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace DevKit;

public class SubModule : MBSubModuleBase
{
    private static Harmony HarmonyInstance { get; set; }

    private GameKey _openWindowManagerKey;
    private GameKey _openMobilePartyDebuggerKey;
    private GameKey _openCampaignEventsDebuggerKey;
    private GameKey _openMissionDebuggerKey;

    protected override void OnSubModuleLoad()
    {
        HarmonyInstance = new Harmony("mod.harmony.devkit");
        HarmonyInstance.PatchAll();
        UIConfig.DoNotUseGeneratedPrefabs = true;

        DevKitHotKeyManager.Initialize();
        var devkitCategory = HotKeyManager.GetCategory(nameof(DevKitGameKeyContext));
        _openWindowManagerKey = devkitCategory.GetGameKey("OpenWindowManager");
        _openMobilePartyDebuggerKey = devkitCategory.GetGameKey("OpenMobilePartyDebugger");
        _openCampaignEventsDebuggerKey = devkitCategory.GetGameKey("OpenCampaignEventsDebugger");
        _openMissionDebuggerKey = devkitCategory.GetGameKey("OpenMissionDebugger");
    }

    protected override void OnApplicationTick(float dt)
    {
        if (Input.IsKeyPressed(_openWindowManagerKey.KeyboardKey.InputKey))
            DebuggerWindows.WindowManager.Toggle();

        if (Input.IsKeyPressed(_openMobilePartyDebuggerKey.KeyboardKey.InputKey))
            DebuggerWindows.MobilePartyDebugger.Toggle();

        if (Input.IsKeyPressed(_openCampaignEventsDebuggerKey.KeyboardKey.InputKey))
            DebuggerWindows.CampaignEventsDebugger.Toggle();

        if (Input.IsKeyPressed(_openMissionDebuggerKey.KeyboardKey.InputKey))
            DebuggerWindows.MissionDebugger.Toggle();
    }
}
