using Newtonsoft.Json;

namespace ModList
{
    public class PluginManifest
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            private set;
        }

        [JsonProperty("version_number")]
        public string Version
        {
            get;
            private set;
        }

        [JsonProperty("website_url")]
        public string Website
        {
            get;
            private set;
        }

        [JsonProperty("description")]
        public string Description
        {
            get;
            private set;
        }

        [JsonProperty("dependencies")]
        public string[] Dependencies
        {
            get;
            private set;
        }
    }
}
