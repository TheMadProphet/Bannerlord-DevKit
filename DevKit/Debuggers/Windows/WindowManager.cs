using System.Collections.Generic;
using System.Diagnostics;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace DevKit.Debuggers.Windows;

public class WindowManager : DebuggerWindow
{
    public override string Name => "Modding Development Kit";

    protected override void Render()
    {
        Imgui.Text("Campaign Windows");
        Imgui.Separator();
        Button("Campaign Events", () => DebuggerWindows.CampaignEventsDebugger.Toggle());
        Button("Mobile Party Debugger", () => DebuggerWindows.MobilePartyDebugger.Toggle());

        Imgui.NewLine();

        Imgui.Text("Mission Windows");
        Imgui.Separator();
        Button("Mission Debugger", () => DebuggerWindows.MissionDebugger.Toggle());

        Imgui.NewLine();

        Imgui.Text("Other");
        Imgui.Separator();
        Button(
            "Debugger.Break()",
            Break,
            "Break into the debugger (if attached) to evaluate custom code / game state."
        );
        Imgui.SameLine(0, 10);
        Button("Print Modules", PrintMods, "Prints all loaded submodules.");
    }

    private static void Break()
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }
    }

    private static void PrintMods()
    {
        var moduleInfos = ModuleHelper.GetModuleInfos(
            TaleWorlds.Engine.Utilities.GetModulesNames()
        );

        InformationManager.DisplayMessage(new InformationMessage("Current Modules:"));
        foreach (var moduleInfo in moduleInfos)
        {
            InformationManager.DisplayMessage(
                new InformationMessage($"  - {moduleInfo.Name} ({moduleInfo.Version})")
            );
        }
    }

    [CommandLineFunctionality.CommandLineArgumentFunction("manager", "devkit")]
    public static string ToggleWindow(List<string> args)
    {
        DebuggerWindows.WindowManager.Toggle();

        return "Toggled DevKit Debuggers window.";
    }
}
