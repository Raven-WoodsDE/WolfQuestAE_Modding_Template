using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using WolfQuestEp3;
using System.Collections.Generic;

namespace Template
{
    [BepInPlugin("com.rw.template", "Template", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<KeyCode> cfgToggleControls;
        public static ConfigEntry<int> cfgMaxLitterSize;
        public static ConfigEntry<float> cfgPupWeightMultiplier;
        public static ConfigEntry<bool> cfgNoScentPostRequirement;

        static List<string> lstSpecies = new List<string>() { "Raven", "Golden Eagle", "Bald Eagle", "Coyote", "Fox" };

        private void Awake()
        {
            // HOW TO CREATE A CONFIG
            cfgToggleControls = Config.Bind("Toggle Controls", "Input, Camera, Cursor; for demonstration", KeyCode.Alpha1, "");
            cfgNoScentPostRequirement = Config.Bind("No Scent Post Requirement", "", false, "");
            cfgMaxLitterSize = Config.Bind("Max Litter Size", "", 0, "");
            cfgPupWeightMultiplier = Config.Bind("Pup Settings", "Weight Gain Multiplier", 1f, "Multiplier for pup weight gain (0.01 to 100)");

            // ALWAYS DO THIS , OTHERWISE YOUR PATCHES WON'T WORK
            Harmony.CreateAndPatchAll(typeof(Plugin));
            Harmony.CreateAndPatchAll(typeof(Patches));

            // THIS IS JUST TO SHOW THAT THE PLUGIN HAS LOADED, YOU CAN REMOVE THIS
            Logger.LogInfo("[Template] Initialised.");
        }

        void Update()
        {
            if (Input.GetKeyDown(cfgToggleControls.Value))
            {
                InputControls.ForceAllowCursor = !InputControls.ForceAllowCursor;
                InputControls.DisableCameraInput = !InputControls.DisableCameraInput;
                InputControls.DisableInput = !InputControls.DisableInput;
            }
        }

        public static bool IsBannedSpecies(string speciesName)
        {
            return lstSpecies.Contains(speciesName);
        }
}