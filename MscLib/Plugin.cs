using MscLib.Types;
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
                bukkitVersions.Select(v => $"versions:{v.GetVersionString()}").ToList()
            };
            var facetsString = JsonConvert.SerializeObject(facets);

            var response = await RestClient.GetAsync($"{APIBaseURL}/search?query={query}&facets={facetsString}&index=newest&limit={limit}");
            var json = JObject.Parse(response)["hits"];
            return json != null
                ? json.ToObject<Plugin[]>() ?? Array.Empty<Plugin>()
                : Array.Empty<Plugin>();
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
            var response = await RestClient.GetAsync($"{APIBaseURL}/project/{Id}/version");
            if (string.IsNullOrEmpty(response)) return Array.Empty<PluginVersion>();
            return JsonConvert.DeserializeObject<List<PluginVersion>>(response).ToArray();
        }

        public async Task SetVersionListAsync() {
            var response = await RestClient.GetAsync($"{APIBaseURL}/project/{Id}/version");
            if (string.IsNullOrEmpty(response)) return;
            PluginVersions = JsonConvert.DeserializeObject<List<PluginVersion>>(response).ToArray();
        }
    }
}
