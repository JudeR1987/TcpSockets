using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NetProgTask1Task2.Views;
/// <summary>
/// Логика взаимодействия для RangeWindow.xaml
/// </summary>
public partial class RangeWindow : Window
{
    // минимальный курс валюты
    private double _low;
    public double Low {
        get => _low;
        set => _low = value;
    } // Low


    // максимальный курс валюты
    private double _high;
    public double High {
        get => _high;
        set => _high = value;
    } // High


    // список курсов валют
    private List<double> _valutes = new();
    public List<double> Valutes {
        get => _valutes;
        set => _valutes = value;
    } // Valutes


    // конструктор по умолчанию
    public RangeWindow() {
    } // RangeWindow()

    // конструктор с параметрами
    public RangeWindow(List<double> allValutesValues) {

        InitializeComponent();

        _valutes = allValutesValues;

        // Комбо-бокс для выбора минимального количества дней
        CbxLow.ItemsSource = null;
        CbxLow.ItemsSource = _valutes
            .Where(valute => valute != _valutes.Last()).ToList();
        CbxLow.SelectedIndex = 0;

    } // RangeWindow


    // событие успешного завершения работы с окном
    private void Ok_Click(object sender, RoutedEventArgs e) {

        // если параметры не выбраны,
        // остаёмся в окне выбора
        if (CbxLow.SelectedIndex == -1 || CbxHigh.SelectedIndex == -1) {

            CbxLow.SelectedIndex = 0;
            return;

        } // if

        // вернуть заданные параметры
        Low = (double)CbxLow.SelectedItem;
        High = (double)CbxHigh.SelectedItem;

        DialogResult = true;

        Close();

        e.Handled = true;

    } // Ok_Click


    // обработчик события выбора в комбо-боксе
    // - выбор минимального курса валюты
    private void CbxLow_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        // если элемент не выбран, завершаем событие
        if (CbxLow.SelectedIndex == -1) return;

        // получим список допустимых значений максимального курса валюты
        var valutesHigh = _valutes
            .Where(valute => valute > (double)CbxLow.SelectedItem)
            .ToList();

        // Комбо-бокс для выбора максимального курса валюты
        CbxHigh.ItemsSource = null;
        CbxHigh.ItemsSource = valutesHigh;
        CbxHigh.SelectedIndex = valutesHigh.Count - 1;

    } // CbxLow_SelectionChanged

} // partial class RangeWindow