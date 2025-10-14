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

    private GameKey _openWindowsManagerKey;

    protected override void OnSubModuleLoad()
    {
        HarmonyInstance = new Harmony("mod.harmony.devkit");
        HarmonyInstance.PatchAll();
        UIConfig.DoNotUseGeneratedPrefabs = true;

        DevKitHotKeyManager.Initialize();
        _openWindowsManagerKey = HotKeyManager
            .GetCategory(nameof(DevKitGameKeyContext))
            .GetGameKey("OpenWindowManager");
    }

    protected override void OnApplicationTick(float dt)
    {
        if (Input.IsKeyPressed(_openWindowsManagerKey.KeyboardKey.InputKey))
        {
            DebuggerWindows.WindowManager.Toggle();
        }
    }
}
