using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetProgTask1Task2.Controllers;

// Контроллер для выполнения обработок по задаче 1
public class Task1Controller
{
    // URI файла для первой задачи
    public string FileUri { get; set; }


    // URI страницы для второй задачи
    public string PageUri { get; set; }


    // конструктор по умолчанию
    public Task1Controller() : this(
        "https://github.com/twbs/bootstrap/releases/download/v4.6.1/bootstrap-4.6.1-examples.zip",
        "https://www.newtonsoft.com/json") {
    } // Task1Controller()

    // конструктор с параметрами
    public Task1Controller(string fileUri, string pageUri) {

        FileUri = fileUri;
        PageUri = pageUri;

    } // Task1Controller


    // Загрузка файла с заданного URI с использованием WebClient
    // (асинхронное выполнение операции загрузки)
    // возвращает количество фактически загруженных байт
    public Task<int> DownloadFile () {

        return Task.Run(async () => {

            // WebClient - устаревшая технология, используем только 
            // в обучающих целях
            var webClient = new WebClient();

            var items = FileUri.Split('/', StringSplitOptions.None);
            var fileName = "App_Files\\" + items[items.Length - 1];

            // асинхронная загрузка файла
            await webClient.DownloadFileTaskAsync(FileUri, fileName);

            // размер загруженного файла
            var size = (int)(new FileInfo(fileName).Length);

            return size;
        });

    } // DownloadFile


    // Загрузка и обработка страницы с указанного URI
    // (асинхронное выполнение операции загрузки)
    public Task<(int LtNumber, int GtNumber)> ProcessPage () {

        return Task.Run(async () => {

            // 1. создать и отправить серверу запрос
            var webRequest = WebRequest.Create(PageUri);


            // 2. ждать ответ сервера, получить ссылку на ответ сервера
            WebResponse webResponse = await webRequest.GetResponseAsync();


            // 3. получение данных от сервера, обработка данных
            int ltNumber = 0, gtNumber = 0;
            var sb = new StringBuilder();

            using (var srd = new StreamReader(webResponse.GetResponseStream())) {

                // чтение данных от сервера
                string str;
                while ((str = (await srd.ReadLineAsync())!) != null) {

                    // обработка очередной строки, полученной от сервера 
                    ltNumber += str.Count(c => c == '<');
                    gtNumber += str.Count(c => c == '>');

                    // запоминаем очередную строку для записи в файл
                    sb.AppendLine(str);

                } // while

            } // using srd


            // 4. закрыть соединение
            webResponse.Close();

            // запись строк в файл (для контроля)
            await File.WriteAllTextAsync(@"App_Files\index.html", sb.ToString(), Encoding.UTF8);

            return (ltNumber, gtNumber);
        });

    } // ProcessPage

} // class Task1Controller