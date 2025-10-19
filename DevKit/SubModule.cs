using DevKit.Debuggers;
using DevKit.Debuggers.Windows;
using DevKit.Hotkeys;
using HarmonyLib;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace DevKit;

public class SubModule : MBSubModuleBase
{
    private static Harmony HarmonyInstance { get; set; }

    private GameKey _openControlPanelKey;
    private GameKey _openMobilePartyDebuggerKey;
    private GameKey _openCampaignEventsDebuggerKey;
    private GameKey _openMissionDebuggerKey;
    private GameKey _openAgentSelectorKey;

    protected override void OnSubModuleLoad()
    {
        HarmonyInstance = new Harmony("mod.harmony.devkit");
        HarmonyInstance.PatchAll();

        DevKitHotKeyManager.Initialize();
        var devkitCategory = HotKeyManager.GetCategory(nameof(DevKitGameKeyContext));

        _openControlPanelKey = devkitCategory.GetGameKey("OpenControlPanel");
        _openMobilePartyDebuggerKey = devkitCategory.GetGameKey("OpenMobilePartyDebugger");
        _openCampaignEventsDebuggerKey = devkitCategory.GetGameKey("OpenCampaignEventsDebugger");
        _openMissionDebuggerKey = devkitCategory.GetGameKey("OpenMissionDebugger");
        _openAgentSelectorKey = devkitCategory.GetGameKey("OpenAgentSelector");
    }

    protected override void OnApplicationTick(float dt)
    {
        var isShiftDown = Input.IsKeyDown(InputKey.LeftShift);
        if (Input.IsKeyPressed(_openControlPanelKey.KeyboardKey.InputKey))
            DebuggerWindows.ControlPanel.Toggle();

        if (Input.IsKeyPressed(_openMobilePartyDebuggerKey.KeyboardKey.InputKey))
        {
            if (isShiftDown)
                DebuggerWindows.AddWindow(new MobilePartyDebugger());
            else
                DebuggerWindows.MobilePartyDebugger.Toggle();
        }

        if (Input.IsKeyPressed(_openCampaignEventsDebuggerKey.KeyboardKey.InputKey))
        {
            if (isShiftDown)
                DebuggerWindows.AddWindow(new CampaignEventsDebugger());
            else
                DebuggerWindows.CampaignEventsDebugger.Toggle();
        }

        if (Input.IsKeyPressed(_openMissionDebuggerKey.KeyboardKey.InputKey))
        {
            if (isShiftDown)
                DebuggerWindows.AddWindow(new MissionDebugger());
            else
                DebuggerWindows.MissionDebugger.Toggle();
        }

        if (Input.IsKeyPressed(_openAgentSelectorKey.KeyboardKey.InputKey))
        {
            if (isShiftDown)
                DebuggerWindows.AddWindow(new AgentSelector());
            else
                DebuggerWindows.AgentSelector.Toggle();
        }
    }
}
