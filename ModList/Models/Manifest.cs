using Newtonsoft.Json;

namespace ModList.Models
{
    public class Manifest
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("version_number")]
        public string VersionNumber { get; set; } = "0.0.0";

        [JsonProperty("dependencies")]
        public string[] Dependencies { get; set; } = {};

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; } = string.Empty;
    }
}
