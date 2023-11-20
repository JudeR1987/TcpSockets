using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text;
using SocketTcpServerTask3.Controllers;

// установим размер окна
#pragma warning disable CA1416 // Проверка совместимости платформы
Console.SetWindowSize(112, 24);
#pragma warning restore CA1416 // Проверка совместимости платформы

Console.Title = "--- TCP Socket Server ---";

// настройка цветового оформления
(Console.ForegroundColor, Console.BackgroundColor) = (ConsoleColor.DarkMagenta, ConsoleColor.Gray);
Console.Clear();

Console.WriteLine("--- TCP Socket Server ---");

// Задать символ "точка" в качестве разделителя целой и дробной 
// частей вещественных чисел
#region для решения проблемы ввода дробных чисел в поля типа number

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

#endregion

// адрес сервера и его порт
var ip = "127.0.0.1";
// var ip = "192.168.0.100";
// var ip = "10.1.196.107";
int tcpPort = 8085;

TcpServer(tcpPort, IPAddress.Parse(ip));

Console.WriteLine("Bye, bye.");
return;


// port порт для работы сервера
// ip   адрес сервера
void TcpServer(int port, IPAddress ip) {

    // конечная точка для сервера: IP и порт
    var ipPoint = new IPEndPoint(ip, port);

    // сокет для прослушивания сети
    // AddressFamily.InterNetwork - IP v4
    // SocketType.Stream          - работаем с пакетами 
    // ProtocolType.Tcp           - протокол транспортного уровня    TCP/IP -- DARPA    IPX/SPX -- Novell
    var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    // Контроллер серверных операций
    var serverController = new ServerController();

    try {
        // привязка сокета к конечной точке
        listenSocket.Bind(ipPoint);

        // прослушиваем сеть по протоколу TCP на заданном порту
        // backlog: максимальная длина очереди ожидающих подключений, в данном случае 10 
        listenSocket.Listen(10);

        Console.WriteLine($"Сервер стартовал, {ip}::{port}, ожидание подключений...");

        // цикл работы сервера
        while (true) {

            // блокирующий вызов, ожидание подключения, после подключения
            // используем сокет handler для обмена данными 
            Socket handler = listenSocket.Accept();

            // клиент обратился с запросом, поэтому Accept() завершен,
            // получим данные клиента - для примера это строка символов
            var data = new byte[1536];      // 1536 - блок данных TCP
            var sbr = new StringBuilder(); // контейнер для декодированного запроса клиента

            // чтение данных клиента из сокета пока есть, что читать
            do {
                // !!!! собственно чтение данных от Клиента !!!!
                var bytes = handler.Receive(data); // количество полученных байтов
                sbr.Append(Encoding.Unicode.GetString(data, 0, bytes));

            } while (handler.Available > 0);

            // обработка клиентского запроса

            // вывод полученного сообщения от клиента 
            Console.WriteLine($"TcpServer {DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}: {sbr}");

            // отправка ответа Клиенту
            string answer;
            var clientCommand = sbr.ToString();
            var tokens = clientCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (tokens[0]) {

                // 1. date – возвращает дату и время на сервере
                case "date":
                    answer = serverController.Date();
                    break;

                // 2. host_name возвращает имя компьютера, на котором работает сервер
                case "host_name":
                    answer = serverController.HostName();
                    break;

                // 3. pwd – возвращает полное имя папки App_Files приложения
                case "pwd":
                    answer = serverController.Pwd();
                    break;

                // 4. list – клиент получает список имен файлов, хранящихся
                // на сервере, в папке App_Files (в папке исполняемого файла),
                // имена файлов разделены строкой “\n”
                case "list":
                    answer = serverController.List();
                    break;

                // 5. mul число1 число2 – сервер возвращает строку, содержащую
                // два вещественных числа и произведение этих чисел
                case "mul":
                    answer = serverController.Mul(tokens);
                    break;

                // 6. sum число1 число2 – сервер возвращает строку, содержащую
                // два вещественных числа и сумму этих чисел
                case "sum":
                    answer = serverController.Sum(tokens);
                    break;

                // 7. solve a b c – сервер возвращает три числа a, b, c
                // и вычисленные корни квадратного уравнения 〖a∙x〗^2+b∙x+c=0,
                // при отсутствии действительных корней возвращать числа a, b,
                // c и строку “\nнет корней\n”
                case "solve":
                    answer = serverController.Solve(tokens);
                    break;

                // 8. div число1 число2 – сервер возвращает строку, содержащую
                // два вещественных числа и частное этих чисел, если число2 == 0,
                // возвращать строку “\ndivide by zero\n
                case "div":
                    answer = serverController.Div(tokens);
                    break;

                // 9. shutdown – завершение работы сервера
                case "shutdown":
                    answer = "halted";
                    break;

                // 10. rename староеИмя новоеИмя – переименование файла в папке
                // App_Files на сервере (в папке исполняемого файла), сервер
                // возвращает “Ok\n”, если файл был переименован на сервере 
                // или строку "Not found\n" – если такого файла на сервере нет
                case "rename":
                    answer = serverController.Rename(tokens);
                    break;

                // 11. upload имяФайла – клиент выбирает файл и отправляет его
                // на сервер, сначала строка “Length ДлинаВБайтах\n”, затем
                // байты файла, далее строка “Ok\n”, сервер сохраняет принятый
                // файл в папку App_Files (в папке исполняемого файла)
                case "upload":
                    answer = serverController.Upload(tokens);
                    break;

                // 12. download имяФайла – сервер передает запрошенный файл
                // из папки App_Files клиенту: строка "Ok ДлинаВБайтах\n"
                // и затем передача байтов файла, далее строка “Ok\n”, если
                // такой файл есть не сервере или строку "Not found\n", если
                // такого файла на сервере нет
                case "download":
                    answer = serverController.Download(tokens);
                    break;

                // 13. delete имяФайла – удаляет файл на сервере, в папке App_Files
                // проекта (в папке исполняемого файла), сервер возвращает
                // “Ok\n”, если файл был удален на сервере или строку
                // "Not found\n" – если такого файла на сервере нет
                case "delete":
                    answer = serverController.Delete(tokens);
                    break;

                // любой другой токен от клиента - просто выводим его в консоль
                default:
                    answer = $"Ok, данные получены {DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}.\nВы прислали \"{clientCommand}\"";
                    break;
            } // if

            // преобразовать ответ в массив байтов и передать клиенту
            data = Encoding.Unicode.GetBytes(answer);
            handler.Send(data);

            // закрыть сокет, используемый для обмена информацией
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            // завершение работы сервера
            if (tokens[0] == "shutdown")
                break;

        } // while

    } catch (Exception ex) {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n\n{ex.Message}\n\n");
        Console.ForegroundColor = ConsoleColor.DarkMagenta;

    } finally {

        listenSocket.Close();

    } // try-catch-finally

} // TcpServer