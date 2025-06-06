using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib.Types {
    public class Plugin {
        private static readonly string APIBaseURL = "https://api.modrinth.com/v2";

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

        public async Task<BukkitVersion[]> GetVersionList() {
            var response = await RestClient.GetAsync($"{APIBaseURL}/project/{Id}/version");
            Console.WriteLine(response);
            var versions = JArray.Parse(response)
                .SelectMany(v => v["game_versions"] ?? new JArray())
                .Where(v => v.Type == JTokenType.String && !string.IsNullOrWhiteSpace(v.ToString()))
                .Select(v => new BukkitVersion(v.ToString()))
                .Distinct()
                .ToArray();

            return versions;
        }

        public async Task DownloadAsync() {
            var response = await RestClient.GetAsync($"{APIBaseURL}/project/{Id}/version");
            var url = JArray.Parse(response)[0]?["files"]?[0]?["url"]?.ToString();
            Console.WriteLine($"Downloading plugin {Title} by {Author} from {url}");
        }
    }
}
