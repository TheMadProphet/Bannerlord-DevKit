using System;
using TaleWorlds.Engine;

namespace DevKit.Debuggers;

public abstract class DebuggerWindow
{
    protected virtual string Name => "Need to override Name";
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
    protected static void WithTooltip(Action content, string tooltip = "")
    {
        content();
        if (Imgui.IsItemHovered() && !string.IsNullOrEmpty(tooltip))
            Imgui.SetTooltip(tooltip);
    }

    #endregion
}
