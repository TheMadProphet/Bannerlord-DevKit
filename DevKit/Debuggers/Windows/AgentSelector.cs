using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace DevKit.Debuggers.Windows;

public class AgentSelector : DebuggerWindow
{
    public override string Name => "DevKit | Agent Selector";
    private static Mission Mission => Mission.Current;

    private bool _onlyShowHumans;

    protected override void Render()
    {
        if (Mission == null)
        {
            Imgui.Text("No active mission.");
            return;
        }

        Imgui.Checkbox("Only Show Humans", ref _onlyShowHumans);
        Imgui.Separator();

        var agentsWithNoTeam = Mission.AllAgents.Where(it => it.Team == null).ToList();
        ListAgents("AttackerTeam", Mission.AttackerTeam?.ActiveAgents);
        ListAgents("AttackerAllyTeam", Mission.AttackerAllyTeam?.ActiveAgents);
        ListAgents("DefenderTeam", Mission.DefenderTeam?.ActiveAgents);
        ListAgents("DefenderAllyTeam", Mission.DefenderAllyTeam?.ActiveAgents);
        ListAgents("Other", agentsWithNoTeam);
    }

    private void ListAgents(string label, List<Agent> agents)
    {
        if (agents == null || agents.Count == 0)
            return;

        if (Imgui.TreeNode(label))
        {
            foreach (var agent in agents)
            {
                if (_onlyShowHumans && !agent.IsHuman)
                    continue;

                Imgui.Text(agent.Name);
                Imgui.SameLine(0, 10);
                SmallButton(
                    $"Select##{agent.Index}",
                    () => DebuggerWindows.AddWindow(new AgentDebugger(agent))
                );

                // SmallButton($"Select##{agent.()}", () => DebuggerWindows.AddWindow(new AgentDebugger(agent)));
                // Imgui.SameLine(0, 10);
                // SmallButton(
                //     "Highlight",
                //     () =>
                //     {
                //         Mission.HighlightedAgents = [agent];
                //     }
                // );
            }

            Imgui.TreePop();
        }
    }
}
