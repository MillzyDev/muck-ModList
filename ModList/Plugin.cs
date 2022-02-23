using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ModList
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, "ModList", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static Harmony harmonyInstance;
        public static ManualLogSource logger;

        private void Awake()
        {
            instance = this;
            harmonyInstance = new Harmony("dev.MillzyG.muck-ModList");
            logger = Logger;

            harmonyInstance.PatchAll();

            // Plugin startup logic
            logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
