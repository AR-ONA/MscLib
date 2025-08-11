// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Versions;

await Core.InitAsync();
var bukkitVersion = new BukkitVersion("1.8.8");
var plugins = await Plugin.SearchPluginsAsync("LuckPerms", 1, [bukkitVersion]);
Bukkit bukkit = await new BukkitBuilder(bukkitVersion)
    .SetMemoryAmount(16384).SetPlugins([plugins[0]]).BuildAsync();
var version = await JavaVersion.GetJavaVersionFromBukkitVersionAsync(bukkit.BukkitVersion);
var pluginVersion = await plugins[0].GetLatestVersionAsync(bukkit.BukkitVersion);
Console.WriteLine($"Plugin version: {pluginVersion.VersionNumber}, Author: {plugins[0].Author}, GameVersion: {String.Join(' ', pluginVersion.GameVersions)}");
Console.WriteLine($"Java Version: {version.majorVersion}, Arch: {version.architecture}, DownloadURL: {version.GetDownloadUrl()}");
Console.WriteLine($"Bukkit Version: {bukkit.BukkitVersion.ToString()}, Memory: {bukkit.MemoryAmount}MB, OS: {bukkit.BukkitVersion.OSType}, Arch: {bukkit.BukkitVersion.Architecture}, DownloadURL: {bukkit.BukkitVersion.GetDownloadUri()}, Path: {bukkit.FilePath}");

await bukkit.CreateServer();

try {
    var manager = new ProcessRunner();

    Console.WriteLine("Starting new Process...");

    var managedServer = manager.StartNewServer(bukkit);

    managedServer.Runner.OnOutputReceived += (log) => {
        if (!string.IsNullOrEmpty(log)) {
            Console.WriteLine($"[SERVER LOG]: {log}");
        }
    };

    managedServer.Runner.OnProcessExited += () => {
        Console.WriteLine($"[SYSTEM]: Server Process (ID: {managedServer.Id}) 가 종료됨");
    };

    Console.WriteLine($"서버가 시작되었습니다 (Managed ID: {managedServer.Id})");
    Console.WriteLine("\n서버가 실행 중입니다.");

    while (managedServer.Runner.IsRunning) {
        var command = Console.ReadLine();
        if (command?.ToLower() == "stop") {
            break;
        }
        await manager.SendCommandToServerAsync(managedServer.Id, command);
    }

    Console.WriteLine("[SYSTEM]: 서버를 종료 중입니다...");
    await manager.StopServerAsync(managedServer.Id);
}
catch (Exception ex) {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n[오류 발생]: 서버를 실행하는 도중 문제가 발생했습니다.");
    Console.WriteLine(ex.ToString());
    Console.ResetColor();
}