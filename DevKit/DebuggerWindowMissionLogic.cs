using DevKit.Debuggers;
using DevKit.Debuggers.Windows;
using TaleWorlds.MountAndBlade;

namespace DevKit;

public class DebuggerWindowMissionLogic : MissionLogic
{
    protected override void OnEndMission()
    {
        foreach (var agentDebugger in WindowManager.GetAllWindows<AgentDebugger>())
        {
            WindowManager.RemoveWindow(agentDebugger);
        }
    }
}
