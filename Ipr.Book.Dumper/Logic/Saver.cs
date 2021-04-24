using System;
using System.IO;
using System.Threading.Tasks;

namespace Ipr.Book.Dumper.Logic {
    public class Saver {
        /// <summary>
        /// Созранение книги в директорию
        /// </summary>
        /// <param name="directory">Директория для сохранение</param>
        /// <param name="name">Имя файла</param>
        /// <param name="bytes">Контент файла в байтах</param>
        /// <returns></returns>
        public static Task Save(string directory, string name, byte[] bytes){
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            var combine = Path.Combine(directory, name);
            Console.WriteLine($"Сохраняем по пути {combine}");
            return File.WriteAllBytesAsync(combine, bytes).ContinueWith(t => Console.WriteLine("Успешно сохранено."));
        }
    }
}