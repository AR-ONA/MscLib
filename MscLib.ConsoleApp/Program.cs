// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Versions;

await Core.InitAsync();
var plugins = await Plugin.SearchPluginsAsync("WorldEdit", 1, [new BukkitVersion("1.21.8")]);
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).SetPlugins([plugins[0]]).BuildAsync();
var version = await JavaVersion.GetJavaVersionFromBukkitVersionAsync(bukkit.BukkitVersion);
foreach (var pv in bukkit.Plugins[0].PluginVersions) {
    Console.WriteLine($"Plugin: {bukkit.Plugins[0].Title}, Version: {pv.VersionNumber}, Game Versions: {string.Join(", ", pv.GameVersions)}, DownloadURL: {pv.GetDownloadUrl()}");
}
Console.WriteLine($"Java Version: {version.majorVersion}, Arch: {version.architecture}, DownloadURL: {version.GetDownloadUrl()}");
Console.WriteLine($"Bukkit Version: {bukkit.BukkitVersion.ToString()}, Memory: {bukkit.GetMemoryAmount()}MB, OS: {bukkit.BukkitVersion.OSType}, Arch: {bukkit.BukkitVersion.Architecture}, DownloadURL: {bukkit.BukkitVersion.GetDownloadUri()}");