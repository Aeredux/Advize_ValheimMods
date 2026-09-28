namespace Advize_PlantEasily;

using HarmonyLib;
using static ModContext;

[HarmonyPatch]
static class HoverTextPatches
{
    static string CurrentModifierKey => ZInput.GamepadActive ? "$KEY_AltKeys" : KeyHintPatches.KeyboardHarvestModifierKeyLocalized;

    static bool ShouldShowHints() => config.ModActive && config.EnableBulkHarvest && config.ShowHoverKeyHints;

    static string BuildHarvestHints(string actionText)
    {
        // Vanilla hover text already ends with the Use binding. Mark that as area harvest,
        // and show the modifier combo as the single-target action.
        return $" (area)\n[<color=yellow><b>{CurrentModifierKey} + $KEY_Use</b></color>] {actionText}";
    }

    [HarmonyPatch(typeof(Beehive), nameof(Beehive.GetHoverText))]
    static void Postfix(Beehive __instance, ref string __result)
    {
        if (!ShouldShowHints())
            return;

        bool isPrivate = !PrivateArea.CheckAccess(__instance.transform.position, 0f, flash: false);
        bool hasHoney = __instance.GetHoneyLevel() > 0;

        if (isPrivate || !hasHoney)
            return;

        string suffix = BuildHarvestHints(__instance.m_extractText);
        __result += Localization.instance.Localize(suffix);
    }

    [HarmonyPatch(typeof(Pickable), nameof(Pickable.GetHoverText))]
    static void Postfix(Pickable __instance, ref string __result)
    {
        if (!ShouldShowHints() || __instance.GetPicked() || __instance.GetEnabled == 0)
            return;

        string suffix = BuildHarvestHints("$inventory_pickup");

        if (config.ReplantOnHarvest && config.ShowHoverReplantHint && ReplantDB.Registry.TryGetValue(Utils.GetPrefabName(__instance.gameObject), out _))
        {
            string hoverName = string.IsNullOrEmpty(PlacementState.LastSelectedPlantName)
                ? __instance.GetHoverName()
                : ReplantDB.Registry[PlacementState.LastSelectedPlantName].Pickable.GetHoverName();

            suffix += $"\nReplant as: <color=green>{hoverName}</color>";
        }

        __result += Localization.instance.Localize(suffix);
    }
}
