using MscLib.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MscLib.Versions {
    public class JavaVersion {
        internal protected static VersionInfo[] JavaVersionsInfo { get; private set; }
        internal static async Task LoadVersionsInfoAsync() {
            string manifestUrl = "https://piston-meta.mojang.com/mc/game/version_manifest_v2.json";
            var jsonString = await RestClient.GetAsync(manifestUrl);

            var manifest = JsonConvert.DeserializeObject<VersionManifest>(jsonString);
            JavaVersionsInfo = manifest.Versions
                .Where(v => v.Type == "release")
                .ToArray();
        }
        public static VersionInfo[] GetVersionInfos() {
            return JavaVersionsInfo;
        }

        public Arch architecture = ArchitectureInfo.GetOperatingSystemArch();        // Default
        public string majorVersion = "17";  // Me too!

        public JavaVersion(string majorVersion) {
            this.majorVersion = majorVersion;
        }

        public async static Task<JavaVersion> GetJavaVersionFromBukkitVersionAsync(BukkitVersion bukkitVersion) {
            var versionInfo = JavaVersionsInfo.FirstOrDefault(v => v.Id == bukkitVersion.VersionString);
            var versionJson = await RestClient.GetAsync(versionInfo.Url);
            
            var majorVersion = JObject.Parse(versionJson).SelectToken("javaVersion.majorVersion").Value<int>();
            
            return new JavaVersion(majorVersion.ToString()) {
                architecture = bukkitVersion.Architecture
            };
        }
    }

    public class VersionManifest {
        [JsonProperty("versions")]
        public List<VersionInfo> Versions { get; set; }
    }

    public class VersionInfo {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

}
