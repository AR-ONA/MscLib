using MscLib.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib {
    public class Core {
        public static async Task InitAsync() {
            await BukkitVersion.LoadVersionsAsync();
            await JavaVersion.LoadVersionsInfoAsync();
        }
    }
}
