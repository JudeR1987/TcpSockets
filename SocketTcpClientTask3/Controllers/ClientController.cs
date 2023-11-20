using SocketTcpClientTask3.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SocketTcpClientTask3.Controllers;

// Клиентские операции
public class ClientController
{
    // адрес сервера и его порт
    private string _ip = "127.0.0.1";
    // string ip = "192.168.0.100";
    // string ip = "10.1.196.107";
    private int _port = 8085;


    // конструктор по умолчанию
    public ClientController() {
    } // ClientController()


    // метод клиентской операции с TCP-сокетом
    public Task TcpClient(string msg, IViewMessage view) {

        return Task.Run(() => {

            var ipServer = IPAddress.Parse(_ip);
            var portServer = _port;

            // конечная точка для клиента - это сервер
            var ipPointServer = new IPEndPoint(ipServer, portServer);

            // сокет для клиента
            // AddressFamily.InterNetwork - IP v4
            // SocketType.Stream          - работаем с пакетами TCP 
            // ProtocolType.Tcp           - протокол транспортного уровня
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                // подключение к серверу - блокирующий вызов до установки соединения
                socket.Connect(ipPointServer);

                // формирование массива байтов для отправки
                byte[] data = Encoding.Unicode.GetBytes(msg);

                // отправка запроса на сервер
                socket.Send(data);

                // получение ответа от сервера
                data = new byte[1536]; // буфер для ответа сервера
                var sbr = new StringBuilder(); // контейнер для декодированного ответа сервера

                // чтение данных сервера из сокета пока есть, что читать
                do {
                    // !!!! собственно чтение данных от Сервера !!!!
                    var bytes = socket.Receive(data, data.Length, 0);
                    sbr.Append(Encoding.Unicode.GetString(data, 0, bytes));

                } while (socket.Available > 0);

                // вывод полученного ответа от сервера
                view.Render($"TcpClient: {sbr}");

                socket.Shutdown(SocketShutdown.Both);

            } // try 
            catch (Exception ex) {

                MessageBox.Show($"{ex.Message}", "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);

            } // catch
            finally {

                socket.Close();

            } // finally

        });

    } // TcpClient

} // class ClientController