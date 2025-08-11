using MscLib.Versions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MscLib {
    public class Plugin {
        private static readonly string APIBaseURL = "https://api.modrinth.com/v2";
        public PluginVersion[] PluginVersions { get; internal set; }

        public async static Task<Plugin[]> SearchPluginsAsync(
            string query,
            int limit = 5,
            BukkitVersion[]? bukkitVersions = null
        ) {
            bukkitVersions ??= Array.Empty<BukkitVersion>();

            var facets = new List<List<string>>
            {
                new List<string> { "project_type:plugin" },
                new List<string> { "categories:paper", "categories:spigot" },
                new List<string> { "categories!=neoforge" },
                new List<string> { "categories!=fabric" },
                bukkitVersions.Select(v => $"versions:{v.GetVersionString()}").ToList()
            };
            var facetsString = JsonConvert.SerializeObject(facets);

            var response = await RestClient.GetAsync($"{APIBaseURL}/search?query={query}&facets={facetsString}&index=newest&limit={limit}");
            var json = JObject.Parse(response)["hits"];

            var plugins = json != null
                ? json.ToObject<Plugin[]>() ?? Array.Empty<Plugin>()
                : Array.Empty<Plugin>();

            return plugins;
        }

        public bool IsDownloaded { get; internal set; } = false;

        [JsonProperty("project_id")]
        public string Id { get; internal set; }
        [JsonProperty("title")]
        public string Title { get; internal set; }
        [JsonProperty("description")]
        public string Description { get; internal set; }
        [JsonProperty("author")]
        public string Author { get; internal set; }
        public Plugin(string id, string title, string description, string version, string author) {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }

        public async Task<PluginVersion[]> GetVersionListAsync() {
            var url = $"{APIBaseURL}/project/{Id}/version?loaders=[\"paper\",\"spigot\"]";

            var response = await RestClient.GetAsync(url);
            if (string.IsNullOrEmpty(response)) return Array.Empty<PluginVersion>();

            return JsonConvert.DeserializeObject<List<PluginVersion>>(response).ToArray();
        }


        public async Task<PluginVersion?> GetLatestVersionAsync(BukkitVersion bukkitVersion) {
            if (PluginVersions == null) {
                PluginVersions = await GetVersionListAsync();
            }
            var versions = PluginVersions
                .Where(v => v.GameVersions.Contains(bukkitVersion.GetVersionString()))
                .OrderByDescending(v => v.VersionNumber)
                .ToArray();
            return versions.FirstOrDefault();
        }

        public async Task DownloadAsync(string filePath, BukkitVersion bukkitVersion) {
            var version = await GetLatestVersionAsync(bukkitVersion);
            if (version == null) return;
            IsDownloaded = true;
            await version.DownloadAsync(filePath);
        }

        public async Task SetVersionListAsync() { PluginVersions = await GetVersionListAsync(); }
    }
}
