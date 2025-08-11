using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib.Types {
    public class BukkitInstance {
        public Guid Id { get; }
        public Bukkit Configuration { get; }
        public JavaRunner Runner { get; }

        public BukkitInstance(Bukkit configuration) {
            Id = Guid.NewGuid();
            Configuration = configuration;
            Runner = new JavaRunner();

            // Runner.OnOutputReceived += (log) => Console.WriteLine($"[{Configuration.ServerName}]: {log}");
        }
    }
}
