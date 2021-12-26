using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Ipr.Book.Dumper.Extensions {
    public static class HttpClientExtensions {
        public static async Task<HtmlDocument> GetHtmlDoc(this HttpClient client, Uri uri) {
            var response = await client.GetAsync(uri);

            if (response.StatusCode != HttpStatusCode.OK) {
                return default;
            }
            
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());

            return doc;
        }
    }
}
