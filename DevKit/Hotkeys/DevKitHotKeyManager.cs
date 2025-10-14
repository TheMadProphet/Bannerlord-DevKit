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
            DevKitGameKeyContext.KeyMap.OpenControlPanel,
            "Toggle Control Panel",
            "Toggles the DevKit Control Panel Window"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenMobilePartyDebugger,
            "Toggle Mobile Party Window",
            "Toggles the Mobile Party Debugger Window"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenCampaignEventsDebugger,
            "Toggle Campaign Events Window",
            "Toggles the Campaign Events Debugger Window"
        );
        SetupHotkey(
            DevKitGameKeyContext.KeyMap.OpenMissionDebugger,
            "Toggle Mission Window",
            "Toggles the Mission Debugger Window"
        );
    }

    private static void SetupHotkey(
        DevKitGameKeyContext.KeyMap key,
        string name,
        string description
    )
    {
        var keyStringId = CategoryId + "_" + (int)key;
        KeyName.AddVariationWithId(keyStringId, new TextObject(name), []);
        KeyDescription.AddVariationWithId(keyStringId, new TextObject(description), []);
    }
}
