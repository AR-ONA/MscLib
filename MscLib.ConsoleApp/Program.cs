// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Types;

await Init();
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).BuildAsync();
Console.WriteLine(bukkit.BukkitVersion.GetVersionString());
Console.WriteLine(bukkit.BukkitVersion.GetBuild());

async Task Init() {
    await MscLib.Core.InitAsync();
    Console.WriteLine("Async 작업 완료");
}