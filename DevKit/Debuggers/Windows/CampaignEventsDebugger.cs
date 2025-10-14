using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace DevKit.Debuggers.Windows;

public class CampaignEventsDebugger : DebuggerWindow
{
    public override string Name => "Campaign Events Debugger";

    private static bool _firstTime = true;
    private static bool _showTimestamps = true;
    private static bool _showArgs = true;

    private static readonly List<(string EventName, object Args, DateTime Timestamp)> EventLog = [];
    public static readonly HashSet<string> Whitelist = [];
    private CampaignEventsWhitelist _whitelistWindow;

    protected override void Render()
    {
        if (Campaign.Current == null || Campaign.Current.MapSceneWrapper == null)
        {
            _firstTime = true;
            EventLog.Clear();
            Imgui.Text("No active campaign.");
            return;
        }

        if (_firstTime && Campaign.Current != null)
        {
            _firstTime = false;
            var events = WrapAllEvents();
            _whitelistWindow = new CampaignEventsWhitelist(events);
        }
        _whitelistWindow.Tick();

        // Controls
        Button("Clear Log", () => EventLog.Clear());
        Imgui.SameLine(0, 10);
        Button("Whitelist...", () => _whitelistWindow.Toggle(), "Open whitelist window");
        Imgui.SameLine(0, 10);
        Imgui.Checkbox("Timestamps", ref _showTimestamps);
        Imgui.SameLine(0, 10);
        Imgui.Checkbox("Args", ref _showArgs);

        // Description
        Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
        Imgui.Text("Displays events fired in the campaign. (Log limit: 100)");
        Imgui.Text("Use the Whitelist button to select which events to log.");
        Imgui.PopStyleColor();

        Imgui.NewLine();

        Collapse(
            "Event Log",
            () =>
            {
                foreach (var (eventName, args, timestamp) in EventLog)
                {
                    if (_showTimestamps)
                    {
                        Imgui.Text($"[{timestamp:HH:mm:ss}]");
                        Imgui.SameLine(0, 10);
                    }

                    Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref YellowStyleColor);
                    Imgui.Text($"{eventName}");
                    Imgui.PopStyleColor();

                    if (args != null && _showArgs)
                    {
                        Imgui.SameLine(0, 10);
                        Imgui.Text($"| Args: {args}");
                    }

                    Imgui.Separator();
                }
            }
        );
    }

    // Wrap all events in CampaignEvents with our own handlers
    private static List<string> WrapAllEvents()
    {
        var events = new List<string>();
        try
        {
            var instanceProperty = typeof(CampaignEvents).GetProperty(
                "Instance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            );

            var campaignEvents = instanceProperty?.GetValue(null);
            if (campaignEvents == null)
                throw new Exception("CampaignEvents.Instance is null");

            var fields = typeof(CampaignEvents).GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                if (
                    !fieldType.IsGenericType
                    || fieldType.GetGenericTypeDefinition() != typeof(MbEvent<>)
                )
                    continue;

                var eventInstance = field.GetValue(campaignEvents);
                var genericArg = fieldType.GetGenericArguments()[0];
                var eventName = field.Name;
                events.Add(eventName);

                var wrapperMethod = typeof(CampaignEventsDebugger).GetMethod(
                    nameof(CreateWrapper),
                    BindingFlags.Static | BindingFlags.NonPublic
                );
                var wrapperMethodGeneric = wrapperMethod.MakeGenericMethod(genericArg);
                var handler = wrapperMethodGeneric.Invoke(null, [eventName]);

                var addMethod = fieldType.GetMethod("AddNonSerializedListener");
                addMethod?.Invoke(eventInstance, [null, handler]);
            }
        }
        catch (Exception ex)
        {
            InformationManager.DisplayMessage(
                new InformationMessage(
                    "MODDING TOOLKIT: Error while wrapping campaign events" + ex,
                    Colors.Red
                )
            );
            InformationManager.DisplayMessage(new InformationMessage(ex.ToString(), Colors.Red));
        }

        return events.OrderBy(e => e).ToList();
    }

    private static Action<T> CreateWrapper<T>(string eventName)
    {
        return data =>
        {
            if (!Whitelist.Contains(eventName))
                return;

            if (EventLog.Count > 100)
                EventLog.RemoveAt(EventLog.Count - 1);

            EventLog.Insert(0, (eventName, data, DateTime.Now));
        };
    }
}
