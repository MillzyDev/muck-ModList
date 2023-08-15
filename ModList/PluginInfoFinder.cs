using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using ModList.Models;
using Newtonsoft.Json;

namespace ModList
{
    public static class PluginInfoFinder
    {
        public static IEnumerable<ModListPlugin> GetInfo()
        {
            HashSet<ModListPlugin> modListPlugins = new();

            foreach (PluginInfo? info in Chainloader.PluginInfos.Values.Where(info => info != null))
            {
                if (LocateManifest(info.Location, out string manifestPath))
                {
                    string content = File.ReadAllText(manifestPath);
                    var manifest = JsonConvert.DeserializeObject<Manifest>(content);

                    // ensure the manifest is correct
                    if (manifest != null)
                    {
                        ModListPlugin manifestInfo = new()
                        {
                            Name = manifest.Name,
                            Description = manifest.Description,
                            Version = manifest.VersionNumber,
                            Dependencies = string.Join(", ", manifest.Dependencies),
                            WebsiteUrl = manifest.WebsiteUrl
                        };

                        modListPlugins.Add(manifestInfo);
                        continue; 
                        // skip the iteration so it will use the chainloader info as a fallback, as well.
                    }
                }

                BepInPlugin metadata = info.Metadata;

                ModListPlugin pluginInfo = new()
                {
                    Name = metadata.Name,
                    Description = info.Location,
                    Version = metadata.Version.ToString(),
                    Dependencies = string.Join(", ",
                        info.Dependencies.Select(dependency => dependency.DependencyGUID)),
                    WebsiteUrl = string.Empty
                };

                modListPlugins.Add(pluginInfo);
            }

            return modListPlugins;
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
