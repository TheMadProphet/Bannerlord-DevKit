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
        if (currentWidget == null)
        {
            return;
        }

        var x = currentWidget.GlobalPosition.X;
        var y = currentWidget.GlobalPosition.Y;
        var num = currentWidget.GlobalPosition.X + currentWidget.Size.X;
        var num2 = currentWidget.GlobalPosition.Y + currentWidget.Size.Y;
        if (x == num || y == num2 || currentWidget.Size.X == 0f || currentWidget.Size.Y == 0f)
            return;

        MBDebug.RenderDebugRectWithColor(x, y, num, num2, 0x2AFF00FF);
    }
}
