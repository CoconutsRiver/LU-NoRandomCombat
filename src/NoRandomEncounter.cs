using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Reflection;

namespace NoRandomCombat
{
    [BepInPlugin("com.CoconutsRiver.NoRandomEncounter", "Disable Random Encounters", "1.0")]
    public class NoRandomEncounter : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            Logger.LogInfo("Disable Random Encounters is loaded!");

            _harmony = new Harmony("com.CoconutsRiver.NoRandomEncounter");
            _harmony.PatchAll();

            Logger.LogInfo("Harmony patches applied successfully.");
        }

        void OnDestroy()
        {
            _harmony.UnpatchSelf();
        }

    }

    [HarmonyPatch(typeof(EnviroModification), "UpdateAll")]
    public class LoadScenePatch
    {
        static void Postfix()
        {
          
                DisableCombatTriggers();
            
        }

        static void DisableCombatTriggers()
        {
            Debug.Log("Disabling combat trigger");
            ///The core of random combat in LU are Ghost Zones trigger areas that are placed everywhere around the map at night
            ///This is a very crude implementation that simply loop through any object containing the cmponent and disabling it
            ///It's not very efficient but the performance hit is moderate at runtime.
            GhostZone[] ghostTriggers = GameObject.FindObjectsOfType<GhostZone>();
            foreach (var ghost in ghostTriggers)
            {
                if (ghost.gameObject != null)
                {
                    ghost.gameObject.GetComponent<BoxCollider>().enabled = false;
                    Debug.Log(ghost.gameObject.name + " Disabled.");
                    
                }
            }
        }
    }


}
