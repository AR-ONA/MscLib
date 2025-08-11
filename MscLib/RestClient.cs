using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MscLib {
    internal class RestClient {
        static readonly HttpClient client = new();

        public static async Task<string> GetAsync(string url) {
            try {
                using var response = await client.GetAsync(new Uri(url));
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) {
                throw new HttpRequestException($"Request failed: {ex.Message}", ex);
            }
        }

        public static async Task<string> DownloadAsync(string url, string targetPathOrDirectory) {
            try {
                using var response = await client.GetAsync(new Uri(url), HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                string fileName;
                if (response.Content.Headers.ContentDisposition?.FileNameStar != null)
                    fileName = response.Content.Headers.ContentDisposition.FileNameStar.Trim('"');
                else if (response.Content.Headers.ContentDisposition?.FileName != null)
                    fileName = response.Content.Headers.ContentDisposition.FileName.Trim('"');
                else {
                    fileName = Path.GetFileName(new Uri(url).AbsolutePath);
                    if (string.IsNullOrEmpty(fileName))
                        fileName = "download.dat";
                }

                string filePath = Directory.Exists(targetPathOrDirectory) ||
                                  string.IsNullOrEmpty(Path.GetExtension(targetPathOrDirectory))
                                  ? Path.Combine(targetPathOrDirectory, fileName)
                                  : targetPathOrDirectory;

                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                await using var stream = await response.Content.ReadAsStreamAsync();
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await stream.CopyToAsync(fileStream);

                return filePath;
            }
            catch (Exception ex) {
                throw new HttpRequestException($"Download failed: {ex.Message}", ex);
            }
        }

        public static Task<string> DownloadAsync(Uri url, string targetPathOrDirectory)
            => DownloadAsync(url.ToString(), targetPathOrDirectory);
    }
}
