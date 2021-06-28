using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Ipr.Book.Dumper.Extensions {
    public static class HttpClientExtensions {
        private const int MAX_TRY_COUNT = 3;

        public static async Task<(string Response, HttpStatusCode StatusCode)> GetStringWithTriesAsync(this HttpClient client, Uri url) {
            for (var i = 0; i < MAX_TRY_COUNT; i++) {
                try {
                    using var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound) {
                        return (string.Empty, response.StatusCode);
                    }
                    
                    if (response.StatusCode != HttpStatusCode.OK) {
                        continue;
                    }

                    return (await response.Content.ReadAsStringAsync(), response.StatusCode);
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            return default;
        }

        public static async Task<HtmlDocument> GetHtmlDoc(this HttpClient client, Uri uri) {
            var (response, statusCode) = await client.GetStringWithTriesAsync(uri);

            if (statusCode != HttpStatusCode.OK) {
                return default;
            }
            
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            return doc;
        }
    }
}
