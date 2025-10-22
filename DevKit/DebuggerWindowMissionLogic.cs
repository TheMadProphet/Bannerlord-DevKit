using DevKit.Debuggers;
using DevKit.Debuggers.Windows;
using TaleWorlds.MountAndBlade;

namespace DevKit;

public class DebuggerWindowMissionLogic : MissionLogic
{
    protected override void OnEndMission()
    {
        foreach (var agentDebugger in DebuggerWindows.GetAllWindows<AgentDebugger>())
        {
            DebuggerWindows.RemoveWindow(agentDebugger);
        }
    }
}
