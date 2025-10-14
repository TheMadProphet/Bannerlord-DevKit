using System.Collections.Generic;
using TaleWorlds.Library;

namespace DevKit.Debuggers.Windows;

public class WindowManager : DebuggerWindow
{
    protected override string Name => "Modding DevKit";

    protected override void Render()
    {
        Button("Mission Debugger", () => DebuggerWindows.MissionDebugger.Toggle());
    }

    [CommandLineFunctionality.CommandLineArgumentFunction("manager", "devkit")]
    public static string ToggleWindow(List<string> args)
    {
        DebuggerWindows.WindowManager.Toggle();

        return "Toggled DevKit Debuggers window.";
    }
}
