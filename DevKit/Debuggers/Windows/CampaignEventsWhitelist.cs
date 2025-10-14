using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace DevKit.Debuggers.Windows;

public class CampaignEventsWhitelist : DebuggerWindow
{
    public override string Name => "Campaign Events Debugger - Whitelist";

    public readonly HashSet<string> Whitelist = [];
    private readonly List<string> _allEvents;

    public CampaignEventsWhitelist(List<string> allEvents)
    {
        _allEvents = allEvents;

        Whitelist.UnionWith(_allEvents);
        Whitelist.ExceptWith(FrequentEvents);
    }

    private static readonly HashSet<string> FrequentEvents =
    [
        "_collectAvailableTutorialsEvent",
        "_partyVisibilityChanged",
        "_onPartySizeChangedEvent",
        "_onPartyConsumedFoodEvent",
        "_onCheckForIssueEvent"
    ];

    protected override void Render()
    {
        if (Whitelist.Count == 0)
            Button("Select All", () => Whitelist.UnionWith(_allEvents));
        else
            Button("Deselect All", () => Whitelist.Clear());

        Imgui.SameLine(0, 10);
        Button("Deselect Tick Events", DeselectTickEvents);
        Imgui.SameLine(0, 10);
        Button(
            "Deselect Frequent Events",
            DeselectFrequentEvents,
            "These events are called very frequently (Default: Off)"
        );
        Imgui.SameLine(0, 10);
        Imgui.Text($"({Whitelist.Count} / {_allEvents.Count})");

        Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
        Imgui.Text("Select which events to log.");
        Imgui.PopStyleColor();

        Imgui.NewLine();

        Imgui.Columns(2);
        foreach (var eventName in _allEvents)
        {
            var isWhitelisted = Whitelist.Contains(eventName);
            StyledCheckbox(eventName, ref isWhitelisted);
            if (!isWhitelisted)
                Whitelist.Remove(eventName);
            else
                Whitelist.Add(eventName);

            Imgui.NextColumn();
        }
    }

    private void DeselectTickEvents()
    {
        var tickEvents = _allEvents.Where(e => e.Contains("Tick") || e.Contains("tick")).ToList();
        Whitelist.ExceptWith(tickEvents);
    }

    private void DeselectFrequentEvents()
    {
        Whitelist.ExceptWith(FrequentEvents);
    }
}
