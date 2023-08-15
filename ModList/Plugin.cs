using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ModList
{
    [BepInPlugin(BuildInfo.Id, BuildInfo.Name, BuildInfo.Version)]
    [BepInProcess("Muck.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony _harmony = null!;

        public static ManualLogSource Log = null!;
        
        private void Awake()
        {
            Log = Logger;
            _harmony = new Harmony(BuildInfo.Id);
        }

        private void Start()
        {
            UiLoader.Instance.LoadUiAssetBundle();
            _harmony.PatchAll();
        }
    }
}
