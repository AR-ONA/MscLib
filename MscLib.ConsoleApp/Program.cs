// See https://aka.ms/new-console-template for more information

using MscLib;
using MscLib.Versions;

await Core.InitAsync();
var plugins = await Plugin.SearchPluginsAsync("WorldEdit", 1, [new BukkitVersion("1.21.8")]);
Bukkit bukkit = await new BukkitBuilder(BukkitVersion.GetBukkitVersions().Last())
    .SetMemoryAmount(16384).SetPlugins([plugins[0]]).BuildAsync();
var version = await JavaVersion.GetJavaVersionFromBukkitVersionAsync(bukkit.BukkitVersion);
var pluginVersion = await plugins[0].GetLatestVersionAsync(bukkit.BukkitVersion);
Console.WriteLine($"Plugin version: {pluginVersion.VersionNumber}, Author: {plugins[0].Author}, GameVersion: {String.Join(' ', pluginVersion.GameVersions)}");
Console.WriteLine($"Java Version: {version.majorVersion}, Arch: {version.architecture}, DownloadURL: {version.GetDownloadUrl()}");
Console.WriteLine($"Bukkit Version: {bukkit.BukkitVersion.ToString()}, Memory: {bukkit.MemoryAmount}MB, OS: {bukkit.BukkitVersion.OSType}, Arch: {bukkit.BukkitVersion.Architecture}, DownloadURL: {bukkit.BukkitVersion.GetDownloadUri()}, Path: {bukkit.FilePath}");

await bukkit.CreateServer();