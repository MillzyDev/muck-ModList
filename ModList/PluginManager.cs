using BepInEx.Bootstrap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModList
{
    class PluginManager
    {
        public static implicit operator bool (PluginManager inst)
        {
            return inst != null;
        }

        private static PluginManager _instance;
        public static PluginManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new PluginManager();
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, BepInEx.PluginInfo> plugins = new();

        public readonly List<string> errors = new();
        public readonly List<Tuple<string, PluginManifest>> pluginData = new();

        private PluginManager()
        {
            errors = Chainloader.DependencyErrors;
            plugins = Chainloader.PluginInfos;

            foreach (var info in plugins)
            {
                string manifestPath = LocateManifest(info.Value.Location);
                PluginManifest manifest = null;

                if (manifestPath != null)
                {
                    string json = File.ReadAllText(manifestPath);
                    if (json != null)
                        manifest = JsonConvert.DeserializeObject<PluginManifest>(json);
                }
                string[] namespaces = info.Key.Split('.');
                string name = namespaces[namespaces.Length - 1];
                pluginData.Add(new Tuple<string, PluginManifest>(name, manifest));
            }

            errors = Chainloader.DependencyErrors;
        }

        private string LocateManifest(string searchFrom)
        {
            try
            {
                string dirName = Path.GetDirectoryName(searchFrom);

                if (File.Exists(Path.Combine(dirName, "manifest.json")))
                    return Path.Combine(dirName, "manifest.json");
                else if (Path.GetDirectoryName(dirName).EndsWith("plugins"))
                    return null;
                else
                    return LocateManifest(dirName);
            }
            catch (Exception e)
            {
                Plugin.GetLogger().LogInfo(e.StackTrace);
            }
            return null;
        }
    }
}
