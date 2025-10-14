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
    public static readonly DebuggerWindow MobilePartyDebugger = new MobilePartyDebugger();
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

    public static void AddWindow(DebuggerWindow window)
    {
        Windows.Add(window);
    }

    public static void RemoveWindow(DebuggerWindow window)
    {
        Windows.Remove(window);
    }
}
