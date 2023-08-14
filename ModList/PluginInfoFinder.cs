using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Bootstrap;

namespace ModList
{
    public static class PluginInfoFinder
    {
        public static IEnumerable<ModListPlugin> GetInfo()
        {
            HashSet<ModListPlugin> modListPlugins = new();

            foreach (PluginInfo? info in Chainloader.PluginInfos.Values)
            {
                if (info == null)
                    continue;

                if (LocateManifest(info.Location, out string manifestPath))
                {
                    // TODO: deserialize manifest
                } else
                {
                    // TODO: use fallback values
                }
            }
        }
        
        private static bool LocateManifest(string searchFrom, out string manifestPath)
        {
            try
            {
                string dirName = Path.GetDirectoryName(searchFrom)!;

                if (File.Exists(Path.Combine(dirName, "manifest.json")))
                {
                    manifestPath = Path.Combine(dirName, "manifest.json");
                    return true;
                }

                if (!Path.GetDirectoryName(dirName)!.EndsWith("plugins"))
                    return LocateManifest(dirName, out manifestPath);
                
                manifestPath = "";
                return false;
            }
            catch (Exception e)
            {
                Plugin.Log.LogInfo(e.StackTrace);
            }

            manifestPath = "";
            return false;
        }
    }
}
