using IPA;
using IPALogger = IPA.Logging.Logger;

using HarmonyLib;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace PauseGuard
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        static float pauseTime = 0;
        
        [Init]
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Plugin.Log.Info("meow");
            Harmony harmony = new Harmony("Catse.BeatSaber.PauseGuard");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

      

        [HarmonyPatch]
        internal class patches
        {

            [HarmonyPatch(typeof(PauseController), "HandlePauseMenuManagerDidPressMenuButton")]
            static bool Prefix()
            {
                return (Time.time > (pauseTime + 1));
            }

            [HarmonyPatch(typeof(PauseController), "HandlePauseMenuManagerDidPressRestartButton")]
            static bool Prefix(PauseController __instance)
            {
                return (Time.time > (pauseTime + 1));
            }

            [HarmonyPatch(typeof(PauseController), "HandlePauseMenuManagerDidPressContinueButton")]
            static bool Prefix(object __instance)
            {
                return (Time.time > (pauseTime + 1));
            }


            [HarmonyPatch(typeof(PauseController), "Pause")]
            static void Postfix(PauseController __instance)
            {
                pauseTime = Time.time;
            }

        }

    }

    
}
