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
    }
}
