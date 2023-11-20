using SocketTcpClientTask3.Controllers;
using SocketTcpClientTask3.Models;
using System.Windows;
using System.Windows.Threading;

namespace SocketTcpClientTask3.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IViewMessage
{
    // Контроллер для обработок по заданию
    private ClientController _clientController;


    // конструктор по умолчанию
    public MainWindow() {

        InitializeComponent();

        _clientController = new ClientController();

        // фокус командной строки
        TbxInput.Text = string.Empty;
        TbxInput.Focus();

    } // MainWindow()


    // завершение работы приложения
    private void Exit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();


    // вывод сведений о приложении и разработчике
    private void About_Click(object sender, RoutedEventArgs e) =>
        new AboutWindow().ShowDialog();


    // метод отправки запроса серверу
    private async void Send_ClickAsync(object sender, RoutedEventArgs e) {

        // пустую строку не вводим
        if (TbxInput.Text == "") return;

        var request = TbxInput.Text;
        TblOutput.Text = $"Команда серверу (shutdown - завершение работы):\n" +
            $"\"{request}\"\n\n{TblOutput.Text}";
        
        await _clientController.TcpClient(request, this);

        // очистка поля ввода
        TbxInput.Text = "";

    } // Send_Click


    #region Короткие команды серверу

    // отправка запроса с командой "DATE"
    private async void Date_ClickAsync(object sender, RoutedEventArgs e) {

        var request = "date";
        TblOutput.Text = $"Команда серверу (shutdown - завершение работы):\n" +
            $"\"{request}\"\n\n{TblOutput.Text}";

        await _clientController.TcpClient(request, this);

        // очистка поля ввода
        TbxInput.Text = "";

    } // Date_ClickAsync


    // отправка запроса с командой "HOST_NAME"
    private async void HostName_ClickAsync(object sender, RoutedEventArgs e) {

        var request = "host_name";
        TblOutput.Text = $"Команда серверу (shutdown - завершение работы):\n" +
            $"\"{request}\"\n\n{TblOutput.Text}";

        await _clientController.TcpClient(request, this);

        // очистка поля ввода
        TbxInput.Text = "";

    } // HostName_ClickAsync


    // отправка запроса с командой "PWD"
    private async void Pwd_ClickAsync(object sender, RoutedEventArgs e) {

        var request = "pwd";
        TblOutput.Text = $"Команда серверу (shutdown - завершение работы):\n" +
            $"\"{request}\"\n\n{TblOutput.Text}";

        await _clientController.TcpClient(request, this);

        // очистка поля ввода
        TbxInput.Text = "";

    } // Pwd_ClickAsync


    // отправка запроса с командой "LIST"
    private async void List_ClickAsync(object sender, RoutedEventArgs e) {

        var request = "list";
        TblOutput.Text = $"Команда серверу (shutdown - завершение работы):\n" +
            $"\"{request}\"\n\n{TblOutput.Text}";

        await _clientController.TcpClient(request, this);

        // очистка поля ввода
        TbxInput.Text = "";

    } // List_ClickAsync

    #endregion


    // вывод сообщения от сервера
    public void Render(string message) {
        
        Dispatcher.BeginInvoke(DispatcherPriority.Normal, () => {

            TblOutput.Text = $"{message}\n\n{TblOutput.Text}";

        }); // BeginInvoke

    } // Render

} // partial class MainWindow