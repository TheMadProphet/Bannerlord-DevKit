using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace DevKit.Debuggers.Windows;

public class MissionDebugger : DebuggerWindow
{
    public override string Name => "DevKit | Mission Debugger";

    private bool _firstTime = true;
    private bool _combatModeIsOpen;
    private bool _combatTypeIsOpen;

    private static Mission Mission => Mission.Current;

    protected override void Render()
    {
        // TODO: Teams
        if (Mission == null || Mission.IsFinalized)
        {
            _firstTime = true;
            Imgui.Text("No active mission.");
            return;
        }

        if (_firstTime && Mission != null)
        {
            _firstTime = false;
            // Init?
        }

        Imgui.Text("Mission");
        Imgui.Separator();
        _combatModeIsOpen = DropdownButton(
            $"Mode: {Mission.Mode}",
            _combatModeIsOpen,
            () =>
            {
                var index = 0;
                foreach (MissionMode mode in Enum.GetValues(typeof(MissionMode)))
                {
                    Button(mode.ToString(), () => Mission.SetMissionMode(mode, false));
                    index++;
                    if (index % 5 != 0)
                        Imgui.SameLine(0, 10);
                }
                Imgui.NewLine();
                Imgui.Separator();
            }
        );
        _combatTypeIsOpen = DropdownButton(
            $"CombatType: {Mission.CombatType}",
            _combatTypeIsOpen,
            () =>
            {
                foreach (
                    Mission.MissionCombatType mode in Enum.GetValues(
                        typeof(Mission.MissionCombatType)
                    )
                )
                {
                    Button(mode.ToString(), () => Mission.SetMissionCombatType(mode));
                    Imgui.SameLine(0, 10);
                }
                Imgui.NewLine();
                Imgui.Separator();
            }
        );
        var battleType = Mission.MissionTeamAIType switch
        {
            Mission.MissionTeamAITypeEnum.FieldBattle => "FieldBattle",
            Mission.MissionTeamAITypeEnum.Siege => "SiegeBattle",
            Mission.MissionTeamAITypeEnum.SallyOut => "SallyOut",
            _ => "<None>"
        };
        Imgui.Text($"Battle Type: {battleType}");
        Imgui.Text($"RequireCivilianEquipment: {Mission.DoesMissionRequireCivilianEquipment}");
        Imgui.Text($"CurrentTime: {Mission.CurrentTime}");

        Imgui.NewLine();

        Imgui.Text("Scene");
        Imgui.Separator();
        Imgui.Text($"SceneName: {Mission.SceneName}");
        Imgui.Text($"SceneLevels: {Mission.SceneLevels}");
        Imgui.Text($"TerrainType: {Mission.TerrainType}");

        Imgui.NewLine();

        Collapse(
            "UI Window Access",
            () =>
            {
                BooleanField("Inventory", Mission.IsInventoryAccessAllowed);
                BooleanField("CharacterWindow", Mission.IsCharacterWindowAccessAllowed);
                BooleanField("QuestScreen", Mission.IsQuestScreenAccessAllowed);
                BooleanField("PartyWindow", Mission.IsPartyWindowAccessAllowed);
                BooleanField("KingdomWindow", Mission.IsKingdomWindowAccessAllowed);
                BooleanField("ClanWindow", Mission.IsClanWindowAccessAllowed);
                BooleanField("EncyclopediaWindow", Mission.IsEncyclopediaWindowAccessAllowed);
                BooleanField("BannerWindow", Mission.IsBannerWindowAccessAllowed);
                Imgui.NewLine();
            }
        );
        Collapse(
            "Mission Behaviors",
            () =>
            {
                foreach (var behavior in Mission.MissionBehaviors)
                {
                    Imgui.Text($"- {behavior.GetType().Name}");

                    if (behavior is MissionLogic)
                    {
                        Imgui.SameLine(0, 10);
                        Text("(MissionLogic)", YellowStyleColor);
                    }
                    else if (behavior is MissionView)
                    {
                        Imgui.SameLine(0, 10);
                        Text("(MissionView)", PurpleStyleColor);
                    }
                }

                Imgui.NewLine();
            }
        );
    }

    private void BooleanField(string label, bool value)
    {
        Imgui.Text($"- {label}:");
        Imgui.SameLine(0, 10);

        if (value)
            Text("Enabled");
        else
            Text("Disabled", GrayStyleColor);
    }
}
