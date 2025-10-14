using System;
using System.Collections.Generic;
using System.Diagnostics;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace DevKit.Debuggers.Windows;

public class ControlPanel : DebuggerWindow
{
    public override string Name => "DevKit Control Panel";

    protected override void Render()
    {
        Imgui.Text("Campaign Windows");
        Imgui.Separator();
        WindowButton<CampaignEventsDebugger>(
            "Campaign Events",
            DebuggerWindows.CampaignEventsDebugger
        );
        WindowButton<MobilePartyDebugger>(
            "Mobile Party Debugger",
            DebuggerWindows.MobilePartyDebugger
        );

        Imgui.NewLine();

        Imgui.Text("Mission Windows");
        Imgui.Separator();
        WindowButton<MissionDebugger>("Mission Debugger", DebuggerWindows.MissionDebugger);

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

    private void WindowButton<T>(string label, DebuggerWindow window)
        where T : DebuggerWindow, new()
    {
        Button(label, window.Toggle);
        Imgui.SameLine(0, 5);
        Button(
            $" + ##{label}",
            () =>
            {
                var newWindow = new T();
                DebuggerWindows.AddWindow(newWindow);
            },
            $"Open additional \"{label}\""
        );
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

    [CommandLineFunctionality.CommandLineArgumentFunction("control_panel", "devkit")]
    public static string ToggleWindow(List<string> args)
    {
        DebuggerWindows.ControlPanel.Toggle();

        return $"Toggled {DebuggerWindows.ControlPanel.Name} window.";
    }
}
