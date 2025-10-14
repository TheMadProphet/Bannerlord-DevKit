using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace DevKit.Hotkeys;

[HarmonyPatch]
public static class DevKitHotKeyManager
{
    private static string CategoryId => nameof(DevKitGameKeyContext);

    private static GameText CategoryName =>
        Module.CurrentModule.GlobalTextManager.GetGameText("str_key_category_name");
    private static GameText KeyName =>
        Module.CurrentModule.GlobalTextManager.GetGameText("str_key_name");
    private static GameText KeyDescription =>
        Module.CurrentModule.GlobalTextManager.GetGameText("str_key_description");

    public static void Initialize()
    {
        var categories = HotKeyManager.GetAllCategories().ToList();
        if (!categories.Any(x => x is DevKitGameKeyContext))
            categories.Add(new DevKitGameKeyContext());

        HotKeyManager.RegisterInitialContexts(categories, true);

        CategoryName.AddVariationWithId(CategoryId, new TextObject("Modding DevKit"), []);

        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenWindowManager,
            "Toggle Window Manager",
            "Toggles the DevKit Window Manager"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenMobilePartyDebugger,
            "Toggle Mobile Party Debugger",
            "Toggles the Mobile Party Debugger Window"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenCampaignEventsDebugger,
            "Toggle Campaign Events Debugger",
            "Toggles the Campaign Events Debugger Window"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenMissionDebugger,
            "Toggle Mission Debugger",
            "Toggles the Mission Debugger Window"
        );
    }

    private static void SetupHotkey(
        DevKitGameKeyContext.KeyMap key,
        string name,
        string description
    )
    {
        var openWindowManagerKey = CategoryId + "_" + (int)key;
        KeyName.AddVariationWithId(openWindowManagerKey, new TextObject(name), []);
        KeyDescription.AddVariationWithId(openWindowManagerKey, new TextObject(description), []);
    }
}
