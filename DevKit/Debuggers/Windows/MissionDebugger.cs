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
                {
                    Imgui.Text($"- {behavior.GetType().Name}");

                    if (behavior is MissionLogic)
                    {
                        Imgui.SameLine(0, 10);
                        Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
                        Imgui.Text("(MissionLogic)");
                        Imgui.PopStyleColor();
                    }
                }

                Imgui.NewLine();
            }
        );
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
    }

    private void BooleanField(string label, bool value)
    {
        Imgui.Text($"- {label}:");
        Imgui.SameLine(0, 10);
        if (value)
        {
            Imgui.Text("Enabled");
        }
        else
        {
            Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
            Imgui.Text("Disabled");
            Imgui.PopStyleColor();
        }
    }
}
