// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Versions;

await Init();
var plugins = await Plugin.SearchPluginsAsync("WorldEdit", 1, [new BukkitVersion("1.21.5")]);
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).SetPlugins([plugins[0]]).BuildAsync();
foreach (var plugin in bukkit.Plugins) {
    foreach (var pluginVersion in plugin.PluginVersions) {
        Console.WriteLine(pluginVersion.Files.Last().Url);
    }
}
async Task Init() {
    await Core.InitAsync();
    Console.WriteLine("Async 작업 완료");
}