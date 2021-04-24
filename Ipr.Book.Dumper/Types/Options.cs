using CommandLine;

namespace Ipr.Book.Dumper.Types {
    public class Options {
        [Option("id", Required = true, HelpText = "Идентификатор книги")]
        public long BookId { get; set; }
        
        [Option("save", Required = true, HelpText = "Директория для сохранения книги")]
        public string SavePath { get; set; }
        
        [Option("filename", Required = true, HelpText = "Название файла для сохранения")]
        public string Name { get; set; }

        [Option("proxy", Required = false, HelpText = "Прокси в формате <host>:<port>", Default = "")]
        public string Proxy { get; set; }
        
        [Option("username", Required = false, HelpText = "Имя пользователя от системы")]
        public string Username { get; set; }
        
        [Option("password", Required = false, HelpText = "Пароль от системы")]
        public string Password { get; set; }

        public bool HasCredentials => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        
        public bool HasProxy => !string.IsNullOrWhiteSpace(Proxy);
    }
}