using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SocketTcpClientTask3.Views;
/// <summary>
/// Логика взаимодействия для AboutWindow.xaml
/// </summary>
public partial class AboutWindow : Window
{
    // поле для запоминания параметров кнопки
    private Thickness _margin;
    private double _width;
    private Brush _foreground = null!;
    private Brush _background = null!;


    // конструктор с параметрами
    public AboutWindow() {

        InitializeComponent();

    } // AboutWindow()


    // закрытие окна
    private void Close_Click(object sender, RoutedEventArgs e) => Close();


    #region Изменение цвета надписи на кнопке при перемещении курсора мыши на кнопку

    // при наведении курсора мыши на кнопку
    private void Button_MouseEnter(object sender, MouseEventArgs e) {

        Button btn = (Button)sender;

        // запоминание параметров кнопки
        _margin = btn.Margin;
        _width = btn.Width;
        _foreground = btn.Foreground;
        _background = btn.Background;

        // установка новых значений
        btn.Margin = new Thickness(10, _margin.Top, _margin.Right, _margin.Bottom);
        btn.Width = 140;
        btn.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 255));
        btn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

    } // Button_MouseEnter


    // при покидании кнопки курсором мыши
    private void Button_MouseLeave(object sender, MouseEventArgs e) {

        Button btn = (Button)sender;

        // установим параметры кнопки в исходное состояние
        btn.Margin = _margin;
        btn.Width = _width;
        btn.Foreground = _foreground;
        btn.Background = _background;

    } // Button_MouseLeave

    #endregion

} // partial class AboutWindow