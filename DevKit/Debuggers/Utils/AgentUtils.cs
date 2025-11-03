using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace DevKit.Debuggers.Utils;

public static class AgentUtils
{
    private static readonly HashSet<int> HighlightedAgents = [];
    private static readonly uint HighlightColor = new TaleWorlds.Library.Color(
        0.5f,
        0.2f,
        1f
    ).ToUnsignedInteger();

    public static void HighlightAgent(this Agent agent)
    {
        if (HighlightedAgents.Contains(agent.Index))
        {
            agent.AgentVisuals?.SetContourColor(null);
            HighlightedAgents.Remove(agent.Index);
        }
        else
        {
            agent.AgentVisuals?.SetContourColor(HighlightColor);
            HighlightedAgents.Add(agent.Index);
        }
    }
}
