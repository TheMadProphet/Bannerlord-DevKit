using DevKit.Configuration;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace DevKit.Patches;

[HarmonyPatch]
public class PatchDevelopmentMode
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MBGameManager), "get_IsDevelopmentMode")]
    private static void EnableDevelopmentMode(ref bool __result)
    {
        if (DevKitConfig.EnableDevelopmentMode)
            __result = true;
    }
}
