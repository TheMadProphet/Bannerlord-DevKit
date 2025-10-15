using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace DevKit.Utilities;

[HarmonyPatch]
public class PatchDevelopmentMode
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MBGameManager), "get_IsDevelopmentMode")]
    private static void EnableDevelopmentMode(ref bool __result)
    {
        __result = true;
    }
}
