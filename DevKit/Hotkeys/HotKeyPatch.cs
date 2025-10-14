using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.MountAndBlade.Options;

namespace DevKit.Hotkeys;

[HarmonyPatch]
public class HotKeyPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(OptionsProvider), "GetGameKeyCategoriesList")]
    public static IEnumerable<string> Postfix(IEnumerable<string> __result)
    {
        return __result.AddItem(nameof(DevKitGameKeyContext));
    }
}
