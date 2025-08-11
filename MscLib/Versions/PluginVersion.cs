using Newtonsoft.Json;

namespace MscLib.Versions {
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

        public Uri GetDownloadUrl() {
            return new Uri(Files.FirstOrDefault()?.Url ?? string.Empty);
        }

        public async Task DownloadAsync(string filePath) {
            if (Files.Count == 0) throw new InvalidOperationException("No files available for download.");
            await RestClient.DownloadAsync(GetDownloadUrl(), filePath);
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
