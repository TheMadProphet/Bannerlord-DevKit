using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

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
    }

    [CommandLineFunctionality.CommandLineArgumentFunction("manager", "devkit")]
    public static string ToggleWindow(List<string> args)
    {
        DebuggerWindows.WindowManager.Toggle();

        return "Toggled DevKit Debuggers window.";
    }
}
