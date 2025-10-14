using HarmonyLib;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace DevKit;

public class SubModule : MBSubModuleBase
{
    public static Harmony HarmonyInstance { get; private set; }

    protected override void OnSubModuleLoad()
    {
        HarmonyInstance = new Harmony("mod.harmony.devkit");
        HarmonyInstance.PatchAll();
        UIConfig.DoNotUseGeneratedPrefabs = true;

        Module.CurrentModule.AddInitialStateOption(
            new InitialStateOption(
                "Message",
                new TextObject("Message"),
                9990,
                () =>
                {
                    InformationManager.DisplayMessage(new InformationMessage("Hello World!"));
                },
                () => (false, null)
            )
        );
    }
}
