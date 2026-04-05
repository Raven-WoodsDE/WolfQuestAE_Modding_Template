using HarmonyLib;
using UnityEngine;
using WolfQuestEp3;

namespace Template
{
    class Patches
    {
        [HarmonyPatch(typeof(TroubleshootingSceneManager), "Awake")]
        [HarmonyPrefix]
        public static bool TroubleshootingSceneManager_Awake_Prefix(TroubleshootingSceneManager __instance)
        {
            Time.timeScale = 1f;
            __instance.canvasGroup.alpha = 0f;
            PlayerPrefs.SetInt("TroubleshootingSceneNeeded", 0);
            SceneUtilities.LoadSceneAsync(SceneUtilities.SceneIdentity.MAIN_MENU, true);
            return false;
        }

        [HarmonyPatch(typeof(HungerAndThirstUpdater), "UpdateGrowth")]
        [HarmonyPrefix]
        public static void HungerAndThirstUpdater_UpdateGrowth_Prefix(ref float hourStep)
        {
            hourStep *= Plugin.cfgPupWeightMultiplier.Value;
        }

        [HarmonyPatch(typeof(LitterCalculator), "GetInitialPupCount")]
        [HarmonyPostfix]
        public static void LitterCalculator_GetInitialPupCount_Postfix(ref int __result)
        {
            if (Plugin.cfgMaxLitterSize.Value > 0)
                __result = Plugin.cfgMaxLitterSize.Value;
        }

        [HarmonyPatch(typeof(PlayerNotificationControls), "OnPlayerNeedsMarkingPost")]
        [HarmonyPrefix]
        public static bool PlayerNotificationControls_OnPlayerNeedsMarkingPost_Prefix()
        {
            return !Plugin.cfgNoScentPostRequirement.Value;
        }

        [HarmonyPatch(typeof(FlockMemberSpawner), "ShouldSpawn")]
        [HarmonyPrefix]
        public static bool FlockMemberSpawner_ShouldSpawn_Prefix(ref bool __result, Flock ___flock)
        {
            if (___flock?.Species == null) return false;

            if (Plugin.IsBannedSpecies(___flock.Species.name))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}