using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace DevKit.Debuggers.Windows;

public class MissionDebugger : DebuggerWindow
{
    public override string Name => "DevKit | Mission Debugger";
    private bool _firstTime = true;

    private static Mission Mission => Mission.Current;

    protected override void Render()
    {
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

        Imgui.Text($"SceneName: {Mission.SceneName}");
        Imgui.Text($"SceneLevels: {Mission.SceneLevels}");
        Imgui.Text($"TerrainType: {Mission.TerrainType}");
        Imgui.NewLine();

        Imgui.Text($"Mode: {Mission.Mode}");
        Imgui.Text($"CombatType: {Mission.CombatType}");
        var battleType = Mission.MissionTeamAIType switch
        {
            Mission.MissionTeamAITypeEnum.FieldBattle => "FieldBattle",
            Mission.MissionTeamAITypeEnum.Siege => "SiegeBattle",
            Mission.MissionTeamAITypeEnum.SallyOut => "SallyOut",
            _ => "<None>"
        };
        Imgui.Text($"Battle Type: {battleType}");
        Imgui.Text($"CurrentTime: {Mission.CurrentTime}");

        Imgui.Text($"RequireCivilianEquipment: {Mission.DoesMissionRequireCivilianEquipment}");
        Imgui.Text($"EnemyAlarmStateIndicator: {Mission.EnemyAlarmStateIndicator}");

        Imgui.NewLine();

        Collapse(
            "Mission Behaviors",
            () =>
            {
                foreach (var behavior in Mission.MissionBehaviors)
                    Imgui.Text($"{behavior.GetType().Name}");
                Imgui.NewLine();
            }
        );
        Collapse(
            "Mission Logics",
            () =>
            {
                foreach (var behavior in Mission.MissionLogics)
                    Imgui.Text($"{behavior.GetType().Name}");
                Imgui.NewLine();
            }
        );
        Collapse(
            "UI Window Access",
            () =>
            {
                Imgui.Text($"Inventory: {Mission.IsInventoryAccessAllowed}");
                Imgui.Text($"CharacterWindow: {Mission.IsCharacterWindowAccessAllowed}");
                Imgui.Text($"QuestScreen: {Mission.IsQuestScreenAccessAllowed}");
                Imgui.Text($"PartyWindow: {Mission.IsPartyWindowAccessAllowed}");
                Imgui.Text($"KingdomWindow: {Mission.IsKingdomWindowAccessAllowed}");
                Imgui.Text($"ClanWindow: {Mission.IsClanWindowAccessAllowed}");
                Imgui.Text($"EncyclopediaWindow: {Mission.IsEncyclopediaWindowAccessAllowed}");
                Imgui.Text($"BannerWindow: {Mission.IsBannerWindowAccessAllowed}");
                Imgui.NewLine();
            }
        );
    }
}
