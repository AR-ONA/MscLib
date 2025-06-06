using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace MscLib.Types {
    public class BukkitVersion {
        internal protected static BukkitVersion[] BukkitVersions { get; private set; }
        private static string APIBaseURL = "https://api.papermc.io/v2/";
        internal protected static async Task LoadVersionsAsync() {
            try {
                var data = await RestClient.GetAsync($"{APIBaseURL}projects/paper");
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                if (obj == null || !obj.TryGetValue("versions", out var versionsObj)) {
                    BukkitVersions = Array.Empty<BukkitVersion>(); return;
                }

                var versionStrings = JsonConvert.DeserializeObject<string[]>(versionsObj.ToString() ?? String.Empty) ?? Array.Empty<string>();

                BukkitVersions = versionStrings
                    .Where(v => !string.IsNullOrWhiteSpace(v) && v != "1.13-pre7")
                    .Select(v => new BukkitVersion(v))
                    .ToArray();

                Console.WriteLine($"Loaded {BukkitVersions.Length} versions:");
                foreach (var bukkitVersion in BukkitVersions) Console.WriteLine(bukkitVersion.ToString());
            }
            catch (Exception ex) {
                BukkitVersions = Array.Empty<BukkitVersion>();
            }
        }

        private string VersionString;
        private int Major;
        private int Minor;
        private int Patch;
        private int Build;

        internal BukkitVersion(string version) {
            VersionString = version;
            string[] parts = VersionString.Split('.');
            Major = int.Parse(parts[0]);
            Minor = int.Parse(parts[1]);
            Patch = parts.Length > 2 && int.TryParse(parts[2], out int patch) ? patch : 0;
            Build = 0;
        }

        public string GetVersionString() {
            return VersionString;
        }

        public override string ToString() {
            return VersionString;
        }

        public int GetBuild() {
            return Build;
        }

        internal protected async Task SetBuildAsync() {
            Build = await GetBuildAsync(this);
        }

        internal protected async Task<int> GetBuildAsync(BukkitVersion version) {
            int[] versions;

            var data = await RestClient.GetAsync(string.Format("{0}projects/paper/versions/{1}", APIBaseURL, version.ToString()));

            var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            try {
                versions = JsonConvert.DeserializeObject<int[]>(obj["builds"].ToString());
            }
            catch (Exception ex) {
                versions = [];
            }

            return versions.Last();
        }

        public static BukkitVersion[] GetBukkitVersions() {
            return BukkitVersions.ToArray();
        }
    }
}
