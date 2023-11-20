using System.Collections.Generic;
using System.Windows;

namespace NetProgTask1Task2.Views;
/// <summary>
/// Логика взаимодействия для ValutesWindow.xaml
/// </summary>
public partial class ValutesWindow : Window
{
    // наименование валюты
    private string _valute = string.Empty;
    public string Valute {
        get => _valute;
        set => _valute = value;
    } // Valute


    // конструктор по умолчанию
    public ValutesWindow() {
    } // ValutesWindow()

    // конструктор с параметрами
    public ValutesWindow(List<string> allValutes) {

        InitializeComponent();

        // Комбо-бокс для выбора валюты
        CbxValutes.ItemsSource = null;
        CbxValutes.ItemsSource = allValutes;
        CbxValutes.SelectedIndex = 0;

    } // ValutesWindow


    // событие успешного завершения работы с окном
    private void Ok_Click(object sender, RoutedEventArgs e) {

        // если параметры не выбраны,
        // остаёмся в окне выбора
        if (CbxValutes.SelectedIndex == -1) {

            CbxValutes.SelectedIndex = 0;
            return;

        } // if

        // вернуть заданные параметры
        Valute = (string)CbxValutes.SelectedItem;

        DialogResult = true;

        Close();

        e.Handled = true;

    } // Ok_Click

} // partial class ValutesWindow