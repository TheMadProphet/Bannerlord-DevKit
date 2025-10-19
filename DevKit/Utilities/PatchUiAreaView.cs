using System.Reflection;
using HarmonyLib;
using TaleWorlds.Engine;
using TaleWorlds.GauntletUI.BaseTypes;

namespace DevKit.Utilities;

[HarmonyPatch]
public class PatchUiAreaView
{
    [HarmonyTargetMethod]
    static MethodBase TargetMethod()
    {
        return AccessTools.Method("TaleWorlds.GauntletUI.UIContext+DebugWidgetTreeNode:DrawArea");
    }

    static void Prefix(object __instance)
    {
        var traverse = Traverse.Create(__instance);
        var currentWidget = traverse.Field("_current").GetValue() as Widget;
        if (currentWidget == null || currentWidget.Size.X == 0f || currentWidget.Size.Y == 0f)
            return;

        var left = currentWidget.GlobalPosition.X;
        var bottom = currentWidget.GlobalPosition.Y;
        var right = currentWidget.GlobalPosition.X + currentWidget.Size.X;
        var top = currentWidget.GlobalPosition.Y + currentWidget.Size.Y;

        MBDebug.RenderDebugRectWithColor(left, bottom, right, top, 0x2AFF00FF);
    }
}
