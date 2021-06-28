using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ipr.Book.Dumper.Extensions;
using Ipr.Book.Dumper.Types;

namespace Ipr.Book.Dumper.Logic {
    public class Dumper {
        private readonly HttpClient _client;

        public Dumper(HttpClient client){
            _client = client;
        }

        public async Task<(string, byte[])> Dump(long bookId){
            var bytes = await _client.GetByteArrayAsync($"https://www.iprbookshop.ru/pdfstream.php?publicationId={bookId}&part=null");
            // Если досутпа к книге нет, то отдается контент вот такой вот длины
            if (bytes.Length == 25462) {
                throw new Exception($"Книга {bookId} недоступна");
            }
            
            return (await GetBookName(bookId), Decode(bytes));
        }

        /// <summary>
        /// Получение названия книги
        /// </summary>
        /// <param name="bookId">Идентификатор книги</param>
        /// <returns></returns>
        private async Task<string> GetBookName(long bookId) {
            var name = $"{bookId}.pdf";
            
            var doc = await _client.GetHtmlDoc(new Uri($"https://www.iprbookshop.ru/{bookId}.html"));
            if (doc == default) {
                return name;
            }

            var bookInfoBlock = doc.DocumentNode.GetByFilterFirst("div", "book-information");
            if (!string.IsNullOrWhiteSpace(bookInfoBlock?.GetByFilterFirst("h4", "header-orange")?.InnerText)) {
                name = $"{bookId}. {bookInfoBlock.GetByFilterFirst("h4", "header-orange")?.InnerText}.pdf";
            }
            
            return name;
        }

        /// <summary>
        /// Авторизация в системе iprbookshop
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <exception cref="Exception"></exception>
        public async Task Authorize(string username, string password){
            var pars = new Dictionary<string, string> {
                ["action"] = "login", 
                ["username"] = username, 
                ["password"] = password, 
                ["rememberme"] = "1"
            };
            
            var response = await _client.PostAsync("https://www.iprbookshop.ru/95835", new FormUrlEncodedContent(pars));
            var content = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!content!.Success) {
                throw new Exception($"Не удалось авторизоваться. {content.Message}");
            }
            
            Console.WriteLine("Авторизация прошла успешно.");
        }

        /// <summary>
        /// Расшифровка контента книги
        /// </summary>
        /// <param name="bytes">Контент книги в виде массива байт</param>
        /// <returns></returns>
        private static byte[] Decode(byte[] bytes){
            Console.WriteLine("Получили шифрованный контент. Расшифровываем.");
            
            for (var i = 0; i < bytes.Length; i += 2048) {
                for (var j = i; j < Math.Min(i + 100, bytes.Length - 1); j += 2) {
                    (bytes[j], bytes[j + 1]) = (bytes[j + 1], bytes[j]);
                }
            }
            
            Console.WriteLine("Расшифровали.");

            return bytes;
        }
    }
}