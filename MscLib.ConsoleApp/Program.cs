// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Versions;

await Init();
var plugins = await Plugin.SearchPluginsAsync("WorldEdit", 1, [new BukkitVersion("1.21.5")]);
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).SetPlugins([plugins[0]]).BuildAsync();
await JavaVersion.GetJavaVersionFromBukkitVersionAsync(new BukkitVersion("1.21.8")).ContinueWith(task => {
    var javaVersion = task.Result;
    Console.WriteLine($"Java Version: {javaVersion.majorVersion}, Arch: {javaVersion.architecture}");
});
async Task Init() {
    await Core.InitAsync();
    Console.WriteLine("Async 작업 완료");
}