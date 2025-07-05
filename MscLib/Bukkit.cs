using MscLib.Types;

namespace MscLib {
    public class Bukkit {
        public BukkitVersion BukkitVersion { get; internal set; }
        public Plugin[] Plugins { get; internal set; } = Array.Empty<Plugin>();
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

        public BukkitBuilder SetPlugins(Plugin[] plugins) {
            Bukkit.Plugins = plugins;
            return this;
        }

        public async Task<Bukkit> BuildAsync() {
            await Bukkit.BukkitVersion.SetBuildAsync();
            foreach (Plugin plugin in Bukkit.Plugins) {
                await plugin.SetVersionListAsync();
            }
            return Bukkit;
        }
    }
}
