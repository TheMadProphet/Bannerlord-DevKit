using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using HarmonyLib;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace DevKit.Debuggers.Windows;

public class MobilePartyDebugger : DebuggerWindow
{
    public override string Name => $"Mobile Party Debugger##{_id}";

    private int _id;
    private bool _firstTime = true;
    private MobileParty _mobileParty;
    private MobileParty _lastHoveredParty;

    private bool _showIdButtons = true;
    private bool _showEncyclopediaButtons = true;
    private bool _autoSelectOnHover = false;

    public MobilePartyDebugger(int? id = null)
    {
        _id = id ?? DebuggerWindows.GetAllWindows<DebuggerWindow>().Count();
    }

    protected override void Render()
    {
        if (Campaign.Current == null || Campaign.Current.MapSceneWrapper == null)
        {
            _firstTime = true;
            Imgui.Text("No active campaign.");
            return;
        }

        if (_firstTime && MobileParty.MainParty != null)
        {
            _firstTime = false;
            _mobileParty = MobileParty.MainParty;
            _lastHoveredParty = null;
        }

        Imgui.Checkbox("ID Buttons", ref _showIdButtons);
        Imgui.SameLine(0, 10);
        Imgui.Checkbox("Wiki Links", ref _showEncyclopediaButtons);
        Imgui.SameLine(0, 10);
        Imgui.Checkbox("Auto-Select on Hover", ref _autoSelectOnHover);
        Imgui.Separator();

        Collapse(
            "SELECTED PARTY",
            () =>
            {
                if (_mobileParty == null)
                    Imgui.Text("<Select a party to see details>");
                else
                    DisplayPartyInfo(_mobileParty);
            }
        );

        Imgui.Separator();
        Collapse(
            "SELECT PARTY ...",
            () =>
            {
                Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
                Imgui.Text("Select a party from the list below.");
                Imgui.PopStyleColor();
                Imgui.NewLine();

                Collapse(
                    $"Lords ({MobileParty.AllLordParties.Count})",
                    () => PartyCheckboxList(MobileParty.AllLordParties)
                );
                Collapse(
                    $"Bandits ({MobileParty.AllBanditParties.Count})",
                    () => PartyCheckboxList(MobileParty.AllBanditParties)
                );
                Collapse(
                    $"Caravans ({MobileParty.AllCaravanParties.Count})",
                    () => PartyCheckboxList(MobileParty.AllCaravanParties)
                );
                Collapse(
                    $"Villagers ({MobileParty.AllVillagerParties.Count})",
                    () => PartyCheckboxList(MobileParty.AllVillagerParties)
                );
                Collapse(
                    $"Custom ({MobileParty.AllCustomParties.Count})",
                    () => PartyCheckboxList(MobileParty.AllCustomParties)
                );
            }
        );
    }

    public void OnPartyHover(MobileParty party)
    {
        if (party != MobileParty.MainParty)
            _lastHoveredParty = party;

        if (_autoSelectOnHover)
            _mobileParty = party;
    }

    private void DisplayPartyInfo(MobileParty party)
    {
        if (!party.IsActive)
        {
            Imgui.Text("<Party is not available>");
            return;
        }

        FieldInfo("", party.Name.ToString(), party.StringId, party.StringId);
        var action = CampaignUIHelper.GetMobilePartyBehaviorText(party);
        Imgui.Text(action);

        Imgui.Separator();
        if (_mobileParty == MobileParty.MainParty)
            PlayerPartyActions();
        else
            MobilePartyActions(party);
        Imgui.Separator();
        Imgui.NewLine();

        FieldInfo(
            "Leader",
            party.LeaderHero?.Name?.ToString(),
            party.LeaderHero?.StringId,
            party.LeaderHero?.StringId,
            party.LeaderHero?.EncyclopediaLink
        );
        FieldInfo(
            "Clan",
            party.ActualClan?.Name?.ToString(),
            party.ActualClan?.StringId,
            party.ActualClan?.StringId,
            party.ActualClan?.EncyclopediaLink
        );
        FieldInfo(
            "Kingdom",
            party.ActualClan?.Kingdom?.Name?.ToString(),
            party.ActualClan?.Kingdom?.StringId,
            party.ActualClan?.Kingdom?.StringId,
            party.ActualClan?.Kingdom?.EncyclopediaLink
        );

        Imgui.Text(
            $"Total troops: {party.MemberRoster.TotalManCount} | Wounded: {party.MemberRoster.TotalWounded}"
        );
        Imgui.Text("Morale: " + party.Morale);
        Imgui.Text("Speed: " + party.Speed);
        Imgui.Text(
            $"Food: {party.Food} | Consumption: {party.FoodChange} | Days: {party.GetNumDaysForFoodToLast()}"
        );
        Imgui.Text($"Position: {party.Position2D.X}, {party.Position2D.Y}");
        FieldInfo(
            "Current Settlement",
            party.CurrentSettlement?.Name?.ToString(),
            party.CurrentSettlement?.StringId,
            party.CurrentSettlement?.StringId,
            party.CurrentSettlement?.EncyclopediaLink
        );

        Imgui.NewLine();

        Collapse(
            "Ai Behavior",
            () =>
            {
                Imgui.Text($"DefaultBehavior: {party.Ai.DefaultBehavior}");
                Imgui.Text($"PartyMoveMode: {party.Ai.PartyMoveMode}");
                Imgui.Text($"IsAlerted: {party.Ai.IsAlerted}");
                Imgui.Text($"DoNotMakeNewDecisions: {party.Ai.DoNotMakeNewDecisions}");
                Imgui.NewLine();
            }
        );

        Collapse(
            "Roles",
            () =>
            {
                FieldInfo(
                    "Scout",
                    party.EffectiveScout?.ToString(),
                    party.EffectiveScout?.StringId,
                    party.EffectiveScout?.StringId,
                    party.EffectiveScout?.EncyclopediaLink
                );
                FieldInfo(
                    "Quartermaster",
                    party.EffectiveQuartermaster?.ToString(),
                    party.EffectiveQuartermaster?.StringId,
                    party.EffectiveQuartermaster?.StringId,
                    party.EffectiveQuartermaster?.EncyclopediaLink
                );
                FieldInfo(
                    "Engineer",
                    party.EffectiveEngineer?.ToString(),
                    party.EffectiveEngineer?.StringId,
                    party.EffectiveEngineer?.StringId,
                    party.EffectiveEngineer?.EncyclopediaLink
                );
                FieldInfo(
                    "Surgeon",
                    party.EffectiveSurgeon?.ToString(),
                    party.EffectiveSurgeon?.StringId,
                    party.EffectiveSurgeon?.StringId,
                    party.EffectiveSurgeon?.EncyclopediaLink
                );
            }
        );

        Imgui.NewLine();
    }

    private static void PlayerPartyActions()
    {
        Button(
            "Heal (Cheat)",
            () =>
            {
                var result = CampaignCheats.HealMainParty(new List<string>());
                if (!string.IsNullOrEmpty(result))
                    InformationManager.DisplayMessage(new InformationMessage("Cheat: " + result));
            }
        );
        Imgui.SameLine(0, 10);
        Button(
            (CampaignCheats.MainPartyIsAttackable ? "Is attackable" : "Is not attackable")
                + " (Cheat)",
            () =>
            {
                var result = CampaignCheats.SetMainPartyAttackable(
                    CampaignCheats.MainPartyIsAttackable ? ["0"] : ["1"]
                );
                if (!string.IsNullOrEmpty(result))
                    InformationManager.DisplayMessage(new InformationMessage("Cheat: " + result));
            }
        );
    }

    private static void MobilePartyActions(MobileParty party)
    {
        if (MobileParty.MainParty.CurrentSettlement == null)
        {
            Button("Teleport to party", () => TeleportParty(MobileParty.MainParty, party));
            if (party.CurrentSettlement == null)
            {
                Imgui.SameLine(0, 10);
                Button("Teleport to player", () => TeleportParty(party, MobileParty.MainParty));

                Imgui.SameLine(0, 10);
                Button(
                    "Destroy",
                    () =>
                    {
                        DestroyPartyAction.Apply(null, party);
                    }
                );
            }
        }
    }

    private static void TeleportParty(MobileParty teleporter, MobileParty target)
    {
        var intersectionPoint = target.Position2D;
        if (teleporter.Army != null)
        {
            var attachedParties = teleporter.Army.LeaderParty.AttachedParties;
            foreach (var mobileParty in attachedParties)
            {
                mobileParty.Position2D += intersectionPoint - teleporter.Position2D;
            }
        }
        teleporter.Position2D = intersectionPoint;
        teleporter.Ai.SetMoveModeHold();

        foreach (var mobileParty in MobileParty.All)
            mobileParty.Party.UpdateVisibilityAndInspected();
        foreach (var settlement in Settlement.All)
            settlement.Party.UpdateVisibilityAndInspected();

        MapScreen.Instance.TeleportCameraToMainParty();
    }

    private void PartyCheckboxList(List<MobileParty> parties)
    {
        foreach (var party in parties.OrderBy(it => it.Name.ToString()))
        {
            PartyCheckbox(party);
        }
    }

    private void PartyCheckbox(MobileParty party)
    {
        var isSelected = party == _mobileParty;
        var partyLabel = $"{party.Name} ({party.MemberRoster.TotalManCount})##{party.StringId}";

        Imgui.Checkbox(partyLabel, ref isSelected);
        if (isSelected)
            _mobileParty = party;
        else if (_mobileParty == party)
            _mobileParty = null;
    }

    private static Vec3 UnpackColor(uint color)
    {
        var r = ((color >> 16) & 0xFF) / 255f;
        var g = ((color >> 8) & 0xFF) / 255f;
        var b = (color & 0xFF) / 255f;

        const float mult = 1.5f; // Make colors brighter for better visibility
        return new Vec3(r * mult, g * mult, b * mult, 1);
    }

    private void FieldInfo(
        string label = "",
        string value = "",
        string id = "",
        string textToCopy = "",
        string encyclopediaLink = ""
    )
    {
        if (!string.IsNullOrEmpty(label))
        {
            Imgui.Text($"{label}:");
            Imgui.SameLine(0, 10);
        }

        if (string.IsNullOrEmpty(value))
        {
            Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref GrayStyleColor);
            Imgui.Text("<None>");
            Imgui.PopStyleColor();

            return;
        }

        Imgui.Text(value);

        // Add ID button
        if (_showIdButtons && !string.IsNullOrEmpty(id))
        {
            var color = new Vec3(1, 1, 1, 0.5f);
            Imgui.PushStyleColor(Imgui.ColorStyle.Text, ref color);
            Imgui.SameLine(0, 10);
            SmallButton(
                $"[{id}]##{id}{label}",
                () =>
                {
                    Clipboard.SetText(textToCopy);
                    InformationManager.DisplayMessage(
                        new InformationMessage($"Copied '{textToCopy}' to clipboard")
                    );
                }
            );
            Imgui.PopStyleColor();
        }

        // Add encyclopedia link button
        // Bandits don't have encyclopedia pages
        if (
            _showEncyclopediaButtons
            && !string.IsNullOrEmpty(encyclopediaLink)
            && !_mobileParty.IsBandit
        )
        {
            Imgui.SameLine(0, 7);
            SmallButton(
                $"Wiki##{label}{encyclopediaLink}",
                () => Campaign.Current.EncyclopediaManager.GoToLink(encyclopediaLink)
            );
        }
    }
}

[HarmonyPatch(typeof(MobileParty))]
[HarmonyPatch("TaleWorlds.CampaignSystem.Map.IMapEntity.OnHover")]
public class MobilePartyHoverPatch
{
    public static void Postfix(MobileParty __instance)
    {
        var debuggers = DebuggerWindows.GetAllWindows<MobilePartyDebugger>();
        foreach (var debugger in debuggers)
        {
            debugger.OnPartyHover(__instance);
        }
    }
}
