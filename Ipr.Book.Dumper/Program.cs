using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommandLine;
using Ipr.Book.Dumper.Logic;
using Ipr.Book.Dumper.Types;

namespace Ipr.Book.Dumper {
    class Program {
        private static async Task Main(string[] args) {
            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async options => {
                    using var handler = new HttpClientHandler {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                    };

                    if (options.HasProxy) {
                        var split = options.Proxy.Split(":");
                        handler.Proxy = new WebProxy(split[0], int.Parse(split[1]));
                    }

                    using var client = new HttpClient(handler);

                    var dumper = new Logic.Dumper(client);
                    if (options.HasCredentials) {
                        await dumper.Authorize(options.Username, options.Password);
                    }

                    foreach (var bookId in options.BookIds) {
                        try {
                            var (name, content) = await dumper.Dump(bookId);
                            await Saver.Save(options.SavePath, name, content);
                        } catch (Exception ex) {
                            Console.WriteLine(ex);
                        }
                    }
                });
        }
    }
}