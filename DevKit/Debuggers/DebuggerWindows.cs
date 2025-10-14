using System.Collections.Generic;
using DevKit.Debuggers.Windows;
using HarmonyLib;
using TaleWorlds.ScreenSystem;

namespace DevKit.Debuggers;

[HarmonyPatch]
public static class DebuggerWindows
{
    public static readonly DebuggerWindow WindowManager = new WindowManager();
    public static readonly DebuggerWindow MissionDebugger = new MissionDebugger();
    public static readonly DebuggerWindow CampaignEventsDebugger = new CampaignEventsDebugger();
    public static readonly DebuggerWindow MobilePartyDebugger = new MobilePartyDebugger(0);
    private static readonly List<DebuggerWindow> Windows =
    [
        WindowManager,
        MissionDebugger,
        CampaignEventsDebugger,
        MobilePartyDebugger
    ];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScreenManager), "UpdateLateTickLayers")]
    public static void Tick()
    {
        foreach (var window in Windows)
            window.Tick();
    }

    public static void AddWindow(DebuggerWindow window, bool open = true)
    {
        Windows.Add(window);

        if (open)
            window.Toggle();
    }

    public static void RemoveWindow(DebuggerWindow window)
    {
        Windows.Remove(window);
    }

    public static IEnumerable<T> GetAllWindows<T>()
        where T : DebuggerWindow
    {
        foreach (var window in Windows)
        {
            if (window is T typedWindow)
                yield return typedWindow;
        }
    }
}
