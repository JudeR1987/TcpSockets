using System.Collections.Generic;
using System.Windows;

namespace NetProgTask1Task2.Views;
/// <summary>
/// Логика взаимодействия для CountriesWindow.xaml
/// </summary>
public partial class CountriesWindow : Window
{
    // наименование страны
    private string _country = string.Empty;
    public string Country {
        get => _country;
        set => _country = value;
    } // Country


    // конструктор по умолчанию
    public CountriesWindow() {
    } // CountriesWindow()

    // конструктор с параметрами
    public CountriesWindow(List<string> allCountries) {

        InitializeComponent();

        // Комбо-бокс для выбора страны
        CbxCountries.ItemsSource = null;
        CbxCountries.ItemsSource = allCountries;
        CbxCountries.SelectedIndex = 0;

    } // CountriesWindow


    // событие успешного завершения работы с окном
    private void Ok_Click(object sender, RoutedEventArgs e) {

        // если параметры не выбраны,
        // остаёмся в окне выбора
        if (CbxCountries.SelectedIndex == -1) {

            CbxCountries.SelectedIndex = 0;
            return;

        } // if

        // вернуть заданные параметры
        Country = (string)CbxCountries.SelectedItem;

        DialogResult = true;

        Close();

        e.Handled = true;

    } // Ok_Click

} // partial class CountriesWindow