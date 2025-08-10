using MscLib.Types;
using MscLib.Versions;

namespace MscLib {
    public class Bukkit {
        public BukkitVersion BukkitVersion { get; internal set; }
        public Plugin[] Plugins { get; internal set; } = Array.Empty<Plugin>();
        public JavaVersion JavaVersion { get; internal set; }
        public bool isCreated { get; internal set; } = false;
        internal int MemoryAmount = 4096;

        internal Bukkit(BukkitVersion version) {
            BukkitVersion = version;
        }

        public async Task SetPluginsAsync(Plugin[] plugins) {
            Plugins = plugins;
            foreach (Plugin plugin in Plugins) {
                await plugin.SetVersionListAsync();
            }
        }

        public int GetMemoryAmount() {
            return MemoryAmount;
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

        public async Task<Bukkit> BuildAsync() {
            await Bukkit.BukkitVersion.SetBuildAsync();
            await Bukkit.SetPluginsAsync(Bukkit.Plugins);
            if (Bukkit.JavaVersion == null) {
                Bukkit.JavaVersion = await JavaVersion.GetJavaVersionFromBukkitVersionAsync(Bukkit.BukkitVersion);
            }
            return Bukkit;
        }
    }
}
