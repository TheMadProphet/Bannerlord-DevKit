using System;
using System.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace DevKit.Debuggers.Windows;

public class AgentDebugger(Agent agent) : DebuggerWindow
{
    public override string Name => $"DevKit | Agent | {agent.Name}";

    private bool _stateIsOpen;
    private bool _isHighlighted;

    protected override void Render()
    {
        if (Mission.Current == null || Mission.Current.IsFinalized)
        {
            Imgui.Text("No active mission.");
            return;
        }

        Imgui.Text("Agent: " + agent.Name);
        if (agent.IsHero)
        {
            var hero = (agent.Character as CharacterObject)?.HeroObject;
            if (hero != null)
            {
                Imgui.SameLine(0, 10);
                Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref YellowStyleColor);
                SmallButton(
                    "Hero",
                    () => Campaign.Current.EncyclopediaManager.GoToLink(hero.EncyclopediaLink)
                );
                Imgui.PopStyleColor();
            }
        }
        Imgui.Text(
            $"Health: {agent.Health}/{agent.HealthLimit} ({(int)(agent.Health / agent.HealthLimit * 100f)}%%)"
        );
        _stateIsOpen = DropdownButton(
            "State: " + agent.State,
            _stateIsOpen,
            () =>
            {
                foreach (var mode in Enum.GetValues(typeof(AgentState)))
                {
                    Button(mode.ToString(), () => agent.State = (AgentState)mode);
                    Imgui.SameLine(0, 10);
                }

                Imgui.NewLine();
                Imgui.Separator();
            }
        );
        Imgui.Text(agent.Team?.ToString() ?? "No Team");

        Imgui.NewLine();
        Button(
            "Highlight",
            () =>
            {
                if (!_isHighlighted)
                {
                    var focusedContourColor = new TaleWorlds.Library.Color(
                        0.5f,
                        0.2f,
                        1f
                    ).ToUnsignedInteger();
                    agent.AgentVisuals?.SetContourColor(focusedContourColor);
                }
                else
                {
                    agent.AgentVisuals?.SetContourColor(null);
                }

                _isHighlighted = !_isHighlighted;
            }
        );
        Imgui.SameLine(0, 10);
        Button("Break", Break, "Break into the debugger (if attached)");
        // Buttons: drop items, kill, teleport to/from

        Collapse(
            "Agent Driven Properties",
            () =>
            {
                var properties = agent.AgentDrivenProperties;
                foreach (var prop in properties.GetType().GetProperties())
                {
                    Imgui.Text($"- {prop.Name}: {prop.GetValue(properties)}");

                    if (prop.Name.Contains("Ai") || prop.Name.Contains("AI"))
                    {
                        Imgui.SameLine(0, 10);
                        Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref PurpleStyleColor);
                        Imgui.Text("(Ai)");
                        Imgui.PopStyleColor();
                    }
                }
            }
        );
        Collapse(
            "Action",
            () =>
            {
                Imgui.Columns(2);
                RenderActionInfo(0);
                Imgui.NextColumn();
                RenderActionInfo(1);
                Imgui.Columns(1);
            }
        );

        // Other ideas:
        // Equipment
        // EventControlFlags
        // Formation
        // monster
    }

    private void RenderActionInfo(int channelId)
    {
        Imgui.Text("Channel: " + channelId);
        Imgui.NewLine();
        Imgui.Text("Priority: " + agent.GetCurrentActionPriority(channelId));
        Imgui.Text("Type: " + agent.GetCurrentActionType(channelId));
        Imgui.Text("Stage: " + agent.GetCurrentActionStage(channelId));
        Imgui.Text($"Weight: {agent.GetActionChannelWeight(channelId):F3}");
        Imgui.Text($"Current Weight: {agent.GetActionChannelCurrentActionWeight(channelId):F3}");
        Imgui.Text($"Progress: {agent.GetCurrentActionProgress(channelId):F2}");
    }

    private void Break()
    {
        Debugger.Break();
    }

    #region Cleanup

    protected override void OnInitialize()
    {
        CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, OnMissionEnded);
    }

    private void OnMissionEnded(IMission mission)
    {
        DebuggerWindows.RemoveWindow(this);
    }

    protected override void OnDispose()
    {
        CampaignEvents.OnMissionEndedEvent.ClearListeners(this);
    }

    #endregion
}
