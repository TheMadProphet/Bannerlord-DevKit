using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace DevKit.Debuggers;

public abstract class DebuggerWindow
{
    public virtual string Name => "Need to override Name";
    private bool _isOpen;

    public void Toggle()
    {
        _isOpen = !_isOpen;
    }

    public void Tick()
    {
        if (!_isOpen)
            return;

        Imgui.BeginMainThreadScope();
        Imgui.Begin(Name, ref _isOpen);

        Render();

        Imgui.End();
        Imgui.EndMainThreadScope();
    }

    protected abstract void Render();

    #region Styles

    public static Vec3 GrayStyleColor = new(1, 1, 1, 0.5f);
    public static Vec3 YellowStyleColor = new(1, 1, 0, 0.85f);

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
}
