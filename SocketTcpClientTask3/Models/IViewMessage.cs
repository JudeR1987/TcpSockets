namespace SocketTcpClientTask3.Models;

// интерфейс для организации вывода сообщения от сервера
public interface IViewMessage
{
    void Render(string message);

} // interface IViewMessage