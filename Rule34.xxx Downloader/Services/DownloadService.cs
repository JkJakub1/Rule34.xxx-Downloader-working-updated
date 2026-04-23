using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace R34Downloader.Services
{
    /// <summary>
    /// Download service.
    /// </summary>
    public static class DownloadService
    {
        #region Fields

        private static readonly CookieContainer CookieContainer;
        private static readonly HttpClient Client;

        #endregion

        #region Constructors

        static DownloadService()
        {
            CookieContainer = new CookieContainer();
            
            // Standard GDPR cookies
            CookieContainer.Add(new Cookie("gdpr", "1", "/", ".rule34.xxx"));
            CookieContainer.Add(new Cookie("gdpr-consent", "1", "/", ".rule34.xxx"));

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = CookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/141.0.0.0 Safari/537.36");
            Client.DefaultRequestHeaders.Referrer = new Uri("https://rule34.xxx/");
            Client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
            
            // Optimization for many connections
            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.Expect100Continue = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Downloads and saves a file at the specified path.
        /// </summary>
        /// <param name="url">File url.</param>
        /// <param name="filePath">File path with name.</param>
        public static void Download(string url, string filePath)
        {
            if (!File.Exists(filePath))
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Simple retry logic (3 attempts)
                for (int attempt = 1; attempt <= 3; attempt++)
                {
                    try
                    {
                        var response = Client.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();

                        var data = response.Content.ReadAsByteArrayAsync().Result;
                        File.WriteAllBytes(filePath, data);
                        break; // Success
                    }
                    catch (Exception)
                    {
                        if (attempt == 3)
                        {
                            // Log or ignore on final failure
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(1000 * attempt); // Progressive backoff
                        }
                    }
                }
            }
        }

        #endregion
    }
}
