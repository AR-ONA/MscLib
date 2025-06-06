// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Types;

await Init();
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).BuildAsync();
Console.WriteLine(bukkit.BukkitVersion.GetVersionString());
Console.WriteLine(bukkit.BukkitVersion.GetBuild());
var plugins = await Plugin.SearchPluginsAsync("Worldedit", 1, [new BukkitVersion("1.21.1")]);
if (plugins.Length > 0) {
    Console.WriteLine($"플러그인 ID: {plugins[0].Id}");
    Console.WriteLine($"플러그인 제목: {plugins[0].Title}");
    Console.WriteLine($"플러그인 설명: {plugins[0].Description}");
    Console.WriteLine($"플러그인 작성자: {plugins[0].Author}");
    foreach (var version in await plugins[0].GetVersionList()) {
        Console.WriteLine($"플러그인 버전: {version.GetVersionString()}");
    }
}
else {
    Console.WriteLine("플러그인을 찾을 수 없습니다.");
}
async Task Init() {
    await MscLib.Core.InitAsync();
    Console.WriteLine("Async 작업 완료");
}