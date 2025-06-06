using MscLib.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MscLib {
    public class Bukkit {
        public BukkitVersion BukkitVersion { get; internal set; }
        internal int MemoryAmount = 4096;

        internal Bukkit(BukkitVersion version) {
            BukkitVersion = version;
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

        public async Task<Bukkit> BuildAsync() {
            await Bukkit.BukkitVersion.SetBuildAsync();
            return Bukkit;
        }
    }
}
