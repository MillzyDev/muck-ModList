using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ModList
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, "ModList", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static Harmony harmonyInstance;
        private static ManualLogSource logger;

        public static GameObject ModListUI
        {
            get;
            private set;
        }

        private void Awake()
        {
            instance = this;
            harmonyInstance = new Harmony(Assembly.GetExecutingAssembly().FullName);
            logger = Logger;

            harmonyInstance.PatchAll();

            // Plugin startup logic
            logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            LoadUI();
        }

        private void LoadUI()
        {
            var bundle = GetAssetBundle("modlistui");

            ModListUI = bundle.LoadAsset<GameObject>("ModListCanvas");
        }

        static readonly OSPlatform[] supportedPlatforms = new OSPlatform[]
        {
            OSPlatform.Windows,
            OSPlatform.OSX
        };

        static AssetBundle GetAssetBundle(string name)
        {
            foreach (var platform in supportedPlatforms)
            {
                if (RuntimeInformation.IsOSPlatform(platform))
                {
                    name = $"{name}-{platform.ToString().ToLower()}";
                    goto load;
                }
            }

            throw new PlatformNotSupportedException("Unsupported platform, cannot load AssetBundles");

            load:
            var execAssembly = Assembly.GetExecutingAssembly();

            var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

            using (var stream = execAssembly.GetManifestResourceStream(resourceName))
            {
                return AssetBundle.LoadFromStream(stream);
            }

        }

        public static ManualLogSource GetLogger()
            => logger;
    }
}
