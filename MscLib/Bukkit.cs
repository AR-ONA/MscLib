using MscLib.Types;
using MscLib.Versions;

namespace MscLib {
    public class Bukkit {
        public BukkitVersion BukkitVersion { get; internal set; }
        public Plugin[] Plugins { get; internal set; } = Array.Empty<Plugin>();
        public JavaVersion JavaVersion { get; internal set; }
        public bool isCreated { get; internal set; } = false;
        public string FilePath { get; internal set; } = string.Empty;
        public int MemoryAmount { get; internal set; } = 4096;

        internal Bukkit(BukkitVersion version) {
            BukkitVersion = version;
        }

        public async Task SetPluginsAsync(Plugin[] plugins) {
            Plugins = plugins;
            foreach (Plugin plugin in Plugins) {
                await plugin.SetVersionListAsync();
            }
        }

        public async Task CreateServer() {
            if (isCreated) return;

            isCreated = true;
            if (!string.IsNullOrEmpty(FilePath) && !Directory.Exists(FilePath)) {
                Directory.CreateDirectory(FilePath);
            }
            
            var jrePath = Path.Combine(FilePath, "jre");
            var downloadedPath = await JavaVersion.DownloadJREAsync(jrePath);
            await Utils.UnzipAsync(downloadedPath, jrePath, "jre");
            await BukkitVersion.DownloadBukkitAsync(FilePath);
            foreach (Plugin plugin in Plugins) {
                await plugin.DownloadAsync(Path.Combine(FilePath, "plugins"), BukkitVersion);
            }
        }
    }

    public class BukkitBuilder {
        private Bukkit Bukkit;

        public BukkitBuilder(BukkitVersion bukkitVersion) {
            Bukkit = new Bukkit(bukkitVersion);
        }

        public BukkitBuilder SetMemoryAmount(int memoryAmount) {
            Bukkit.MemoryAmount = memoryAmount;
            return this;
        }

        public BukkitBuilder SetJavaVersion(JavaVersion javaVersion) {
            Bukkit.JavaVersion = javaVersion;
            return this;
        }

        public BukkitBuilder SetPlugins(Plugin[] plugins) {
            Bukkit.Plugins = plugins;
            return this;
        }
        public BukkitBuilder SetArchitecture(Arch arch) {
            Bukkit.BukkitVersion.Architecture = arch;
            return this;
        }

        public BukkitBuilder SetOSType(OSType os) {
            Bukkit.BukkitVersion.OSType = os;
            return this;
        }

        public BukkitBuilder SetLocation(string location) {
            Bukkit.FilePath = location;
            return this;
        }

        public async Task<Bukkit> BuildAsync() {
            if (Bukkit.FilePath == string.Empty) {
                Bukkit.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MscLib", "Bukkit", Bukkit.BukkitVersion.GetVersionString() + "_" + Guid.NewGuid().ToString());
            }
            await Bukkit.BukkitVersion.SetBuildAsync();
            await Bukkit.SetPluginsAsync(Bukkit.Plugins);
            if (Bukkit.JavaVersion == null) {
                Bukkit.JavaVersion = await JavaVersion.GetJavaVersionFromBukkitVersionAsync(Bukkit.BukkitVersion);
            }
            return Bukkit;
        }
    }
}
