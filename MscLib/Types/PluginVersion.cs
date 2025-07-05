using Newtonsoft.Json;

namespace MscLib.Types {
    public class PluginVersion {
        [JsonProperty("id")]
        public string Id { get; internal set; }
        [JsonProperty("game_versions")]
        public List<string> GameVersions { get; internal set; }
        [JsonProperty("name")]
        public string Name { get; internal set; }
        [JsonProperty("version_number")]
        public string VersionNumber { get; internal set; }
        [JsonProperty("version_type")]
        public string VersionType { get; internal set; }
        [JsonProperty("files")]
        public List<PluginFile> Files { get; set; }

        public async Task DownloadAsync() {

        }
    }

    public class PluginFile {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
