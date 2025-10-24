using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevKit.Configuration;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace DevKit.Debuggers.Windows;

public class ControlPanel : DebuggerWindow
{
    public override string Name => "DevKit Control Panel";

    protected override void Render()
    {
        Collapse(
            "Kit Config",
            () =>
            {
                ConfigCheckbox(
                    "Enable Development Mode",
                    ref DevKitConfig.EnableDevelopmentMode,
                    "Enables development mode, used internally by TaleWorlds"
                );
                ConfigCheckbox(
                    "Enable UI Area patch",
                    ref DevKitConfig.EnableUiAreaPatch,
                    "Fixes highlight not working in menu: ui.set_screen_debug_information_enabled"
                );
            }
        );
        Imgui.NewLine();

        Imgui.Text("Campaign Windows");
        Imgui.Separator();
        WindowButton<CampaignEventsDebugger>(
            "Campaign Events",
            WindowManager.CampaignEventsDebugger
        );
        WindowButton<MobilePartyDebugger>(
            "Mobile Party Debugger",
            WindowManager.MobilePartyDebugger
        );

        Imgui.NewLine();

        Imgui.Text("Mission Windows");
        Imgui.Separator();
        WindowButton<MissionDebugger>("Mission Debugger", WindowManager.MissionDebugger);
        WindowButton<MissionDebugger>("Agent Selector", WindowManager.AgentSelector);

        Imgui.NewLine();

        Imgui.Text("Other");
        Imgui.Separator();
        Button(
            "Debugger.Break()",
            Break,
            "Break into the debugger (if attached) to evaluate custom code / game state"
        );
        Imgui.SameLine(0, 10);
        Button("Print Modules", PrintMods, "Prints all loaded submodules in order");
    }

    private void WindowButton<T>(string label, DebuggerWindow mainWindow)
        where T : DebuggerWindow, new()
    {
        Button(label, mainWindow.Toggle);
        Imgui.SameLine(0, 5);
        Button(
            $" + ##{label}",
            () =>
            {
                var newWindow = new T();
                WindowManager.AddWindow(newWindow);
            },
            $"Open additional \"{label}\""
        );

        var openWindows = WindowManager.GetAllWindows<T>().Where(it => it.IsOpen).ToList();
        if (openWindows.Count > 1)
        {
            Imgui.SameLine(0, 5);
            Button(
                $"Close all ({openWindows.Count})",
                () =>
                {
                    foreach (var win in openWindows)
                    {
                        if (win != mainWindow)
                            WindowManager.RemoveWindow(win);
                        else
                            win.IsOpen = false;
                    }
                }
            );
        }
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
        WindowManager.ControlPanel.Toggle();

        return $"Toggled {WindowManager.ControlPanel.Name} window.";
    }
}
