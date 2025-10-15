using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace DevKit.Debuggers;

public abstract class DebuggerWindow : IDisposable
{
    public virtual string Name => "Need to override Name";
    protected readonly int Id;
    public bool IsOpen;

    protected DebuggerWindow()
        : this(null) { }

    protected DebuggerWindow(int? id = null)
    {
        Id = id ?? DebuggerWindows.NextId;
    }

    public void Toggle()
    {
        IsOpen = !IsOpen;
    }

    public void Tick()
    {
        if (!IsOpen)
            return;

        Imgui.BeginMainThreadScope();
        Imgui.Begin($"{Name}##{Id}", ref IsOpen);

        Render();

        Imgui.End();
        Imgui.EndMainThreadScope();
    }

    protected abstract void Render();

    #region Styles

    public static Vec3 GrayStyleColor = new(1, 1, 1, 0.5f);
    public static Vec3 YellowStyleColor = new(1, 1, 0, 0.85f);
    public static Vec3 PurpleStyleColor = new(0.8f, 0.4f, 1f, 0.85f);

    #endregion

    #region Helpers

    protected static void Button(string label, Action onClick, string tooltip = "")
    {
        WithTooltip(
            () =>
            {
                var clicked = Imgui.Button(label);
                if (clicked)
                    onClick();
            },
            tooltip
        );
    }

    protected static void SmallButton(string label, Action onClick)
    {
        var clicked = Imgui.SmallButton(label);
        if (clicked)
            onClick();
    }

    protected bool DropdownButton(string label, bool open, Action content)
    {
        Imgui.Text(label);
        Imgui.SameLine(0, 10);
        var newState = open;
        SmallButton(open ? $" ^ ##{label}" : $" V ##{label}", () => newState = !newState);

        if (newState)
            content();

        return newState;
    }

    protected static void Collapse(string label, Action content)
    {
        var displayItems = Imgui.CollapsingHeader(label);
        if (displayItems)
            content();
    }

    protected static void StyledCheckbox(string label, ref bool value)
    {
        if (value)
            Imgui.Checkbox(label, ref value);
        else
        {
            Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
            Imgui.Checkbox(label, ref value);
            Imgui.PopStyleColor();
        }
    }

    protected static void WithTooltip(Action content, string tooltip = "")
    {
        content();
        if (Imgui.IsItemHovered() && !string.IsNullOrEmpty(tooltip))
            Imgui.SetTooltip(tooltip);
    }

    #endregion

    #region Disposal

    protected virtual void OnDispose() { }

    public void Dispose()
    {
        OnDispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
