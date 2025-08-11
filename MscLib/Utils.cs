using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib {
    internal class Utils {
        public static void Unzip(string zipFilePath, string extractFolderPath, string fixedFolderName = null) {
            ZipFile.ExtractToDirectory(zipFilePath, extractFolderPath);
            if (string.IsNullOrEmpty(fixedFolderName)) return;

            var dirs = Directory.GetDirectories(extractFolderPath);
            if (dirs.Length == 1) {
                var extractedFolder = dirs[0];
                var targetFolder = Path.Combine(extractFolderPath, fixedFolderName);
                if (Directory.Exists(targetFolder)) {
                    Directory.Delete(targetFolder, recursive: true);
                }
                Directory.Move(extractedFolder, targetFolder);
            }
            else {
                Console.WriteLine("Folder Count Overflowed!!");
            }
        }

        public static Task UnzipAsync(string zipFilePath, string extractFolderPath, string fixedFolderName = null) {
            return Task.Run(() => Unzip(zipFilePath, extractFolderPath, fixedFolderName));
        }
    }
}
