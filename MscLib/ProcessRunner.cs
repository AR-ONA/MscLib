using MscLib.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib {
    public class ProcessRunner {
        private readonly ConcurrentDictionary<Guid, BukkitInstance> _runningServers = new();

        public BukkitInstance StartNewServer(Bukkit serverConfig) {
            var serverInstance = new BukkitInstance(serverConfig);

            var javaExecutableName = "java";
            if (serverConfig.BukkitVersion.OSType == OSType.Windows) {
                javaExecutableName = "java.exe";
            }

            var javaExecutablePath = Path.Combine(serverConfig.FilePath, "jre", "jre","bin", javaExecutableName);
            var serverJarPath = Directory.GetFiles(serverConfig.FilePath, "*.jar").First();
            var serverJarFileName = Path.GetFileName(serverJarPath);

            //var aikarsFlags = new List<string>
            //{
            //    "-XX:+UseG1GC",
            //    "-XX:+ParallelRefProcEnabled",
            //    "-XX:MaxGCPauseMillis=200",
            //    "-XX:+UnlockExperimentalVMOptions",
            //    "-XX:+DisableExplicitGC",
            //    "-XX:+AlwaysPreTouch",
            //    "-XX:G1NewSizePercent=30",
            //    "-XX:G1MaxNewSizePercent=40",
            //    "-XX:G1HeapRegionSize=8M",
            //    "-XX:G1ReservePercent=20",
            //    "-XX:G1HeapWastePercent=5",
            //    "-XX:G1MixedGCCountTarget=4",
            //    "-XX:InitiatingHeapOccupancyPercent=15",
            //    "-XX:G1MixedGCLiveThresholdPercent=90",
            //    "-XX:G1RSetUpdatingPauseTimePercent=5",
            //    "-XX:SurvivorRatio=32",
            //    "-XX:MaxTenuringThreshold=1",
            //    "-XX:+PerfDisableSharedMem",
            //    "-Dusing.aikars.flags=https://mcflags.emc.gs",
            //    "-Daikars.new.flags=true"
            //};
            // TODO: Aikar's flags 추가

            serverInstance.Runner.Start(
                javaExecutablePath,
                serverConfig.FilePath,
                serverJarFileName,
                serverConfig.MemoryAmount,
                "nogui"
            );

            _runningServers.TryAdd(serverInstance.Id, serverInstance);

            serverInstance.Runner.OnProcessExited += () => {
                _runningServers.TryRemove(serverInstance.Id, out _);
            };

            return serverInstance;
        }

        public async Task<bool> StopServerAsync(Guid serverId) {
            if (_runningServers.TryGetValue(serverId, out var server)) {
                await server.Runner.StopAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SendCommandToServerAsync(Guid serverId, string command) {
            if (_runningServers.TryGetValue(serverId, out var server)) {
                await server.Runner.SendCommandAsync(command);
                return true;
            }
            return false;
        }

        public ICollection<BukkitInstance> GetAllRunningServers() {
            return _runningServers.Values;
        }
    }
}
