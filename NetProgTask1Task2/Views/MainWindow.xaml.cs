using NetProgTask1Task2.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace NetProgTask1Task2.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // контроллеры для обработки по задачам
    private Task1Controller _task1Controller;
    private Task2Controller _task2Controller;


    // конструктор по умолчанию
    public MainWindow() : this(new Task1Controller(), new Task2Controller()) {
    } // MainWindow()

    // конструктор с параметрами
    public MainWindow(Task1Controller task1Controller, Task2Controller task2Controller) {

        InitializeComponent();

        // получить контроллеры
        _task1Controller = task1Controller;
        _task2Controller = task2Controller;

        // начало работы
        TblInfo.Text = $"[{DateTime.Now:T}]: Приложение готово к работе.\n\n";

        // вывод валют
        RunTask2InfoBold.Text = $" \"{DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}\"";
        UpdateDaraGrid(DgdCurrencies, _task2Controller.Valutes);

    } // MainWindow()


    // завершение работы приложения
    private void Exit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();


    // вывод сведений о приложении и разработчике
    private void About_Click(object sender, RoutedEventArgs e) =>
        new AboutWindow().ShowDialog();


    #region Обработка задачи 1

    // загрузка файла
    private async void FileDownload_ClickAsync(object sender, RoutedEventArgs e) {

        // переход на вкладку
        TbcMain.SelectedIndex = 0;

        // очистка страницы
        TblNewtonSoft.Text = "";

        // загрузка файла
        var size = await _task1Controller.DownloadFile();

        // сообщение
        TblInfo.Text = $"[{DateTime.Now:T}]: Загрузка файла выполнена.\n" +
            $"Размер файла: {size:n0} байт.\n\n{TblInfo.Text}";

        // сообщение в строке состояния
        LblStatus.Content = $"Загружен файл: {size / 1024d:n0} Кбайт.";

    } // FileDownload_ClickAsync


    // загрузка и обработка страницы с указанного URI
    private async void PageProcess_ClickAsync(object sender, RoutedEventArgs e) {

        // переход на вкладку
        TbcMain.SelectedIndex = 0;

        // очистка страницы
        TblNewtonSoft.Text = "";

        // загрузка и обработка
        (int lt, int gt) = await _task1Controller.ProcessPage();

        // сообщение
        TblInfo.Text = $"[{DateTime.Now:T}]: Страница обработана.\n" +
           $"Знаков \"<\": {lt}, знаков \">\": {gt}.\n\n{TblInfo.Text}";

        // вывод загруженной страницы
        TblNewtonSoft.Text = await File.ReadAllTextAsync("App_Files\\index.html", Encoding.UTF8);

        // сообщение в строке состояния
        LblStatus.Content = $"Загружена страница: \"{_task1Controller.PageUri}\" | " +
            $"Знаков \"<\": {lt}, знаков \">\": {gt} |";

    } // PageProcess_ClickAsync

    #endregion


    #region Обработка задачи 2

    // обновить курсы валют
    private async void ValutesRefresh_ClickAsync(object sender, RoutedEventArgs e) {

        // обновить данные
        await _task2Controller.GetValutesAsync();

        // выборка данных по запросу
        var valutes = _task2Controller.Valutes;

        // переход на вкладку
        TbcMain.SelectedIndex = 1;

        // заполнение таблицы
        UpdateDaraGrid(DgdCurrencies, valutes);

        // заполнить информационную строку
        RunTask2Info1.Text = "Курсы валют на ";
        RunTask2InfoBold.Text = $"\"{DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}\"";
        RunTask2Info2.Text = ".";

        // заполнение строки состояния
        LblStatus.Content = $"Количество валют: {valutes.Count}.";

    } // ValutesRefresh_ClickAsync


    // валюты с курсом в заданном диапазоне значений
    private async void ValutesByRange_ClickAsync(object sender, RoutedEventArgs e) {

        // выборка данных для запроса
        var allValutesValues = await _task2Controller.GetAllValutesValues();

        // создать окно выбора параметров
        // если окно закрыто не по OK, завершаем событие
        var rangeWindow = new RangeWindow(allValutesValues);
        rangeWindow.ShowDialog();

        if (rangeWindow.DialogResult != true) return;

        // выборка данных по запросу
        var low = rangeWindow.Low;
        var high = rangeWindow.High;
        var valutes = await _task2Controller.ValutesInRange(low, high);

        // переход на вкладку
        TbcMain.SelectedIndex = 1;

        // заполнение таблицы
        UpdateDaraGrid(DgdCurrencies, valutes);

        // заполнить информационную строку
        RunTask2Info1.Text = "Курсы валют ";
        RunTask2InfoBold.Text = $"от {low:n4} руб. до {high:n4} руб.";
        RunTask2Info2.Text = "";

        // заполнение строки состояния
        LblStatus.Content = $"Количество валют: {valutes.Count}.";

    } // ValutesByRange_ClickAsync


    // курсы валют заданной страны
    private async void ValutesByCountry_ClickAsync(object sender, RoutedEventArgs e) {

        // выборка данных для запроса
        var allCountries = await _task2Controller.GetAllCountries();

        // создать окно выбора параметров
        // если окно закрыто не по OK, завершаем событие
        var countriesWindow = new CountriesWindow(allCountries);
        countriesWindow.ShowDialog();

        if (countriesWindow.DialogResult != true) return;

        // выборка данных по запросу
        var country = countriesWindow.Country;
        var valutes = await _task2Controller.ValuteByCountry(country);

        // переход на вкладку
        TbcMain.SelectedIndex = 1;

        // заполнение таблицы
        UpdateDaraGrid(DgdCurrencies, valutes);

        // заполнить информационную строку
        RunTask2Info1.Text = $"Курс{(valutes.Count == 1 ? "" : "ы")} валют{(valutes.Count == 1 ? "ы" : "")} страны ";
        RunTask2InfoBold.Text = $"\"{country}\"";
        RunTask2Info2.Text = ".";

        // заполнение строки состояния
        LblStatus.Content = $"Количество валют: {valutes.Count}.";

    } // ValutesByCountry_ClickAsync


    // курс заданной валюты
    private async void ValutesByName_ClickAsync(object sender, RoutedEventArgs e) {

        // выборка данных для запроса
        var allValutes = await _task2Controller.GetAllValutes();

        // создать окно выбора параметров
        // если окно закрыто не по OK, завершаем событие
        var valutesWindow = new ValutesWindow(allValutes);
        valutesWindow.ShowDialog();

        if (valutesWindow.DialogResult != true) return;

        // выборка данных по запросу
        var valute = valutesWindow.Valute;
        var valutes = await _task2Controller.ValuteByName(valute);

        // переход на вкладку
        TbcMain.SelectedIndex = 1;

        // заполнение таблицы
        UpdateDaraGrid(DgdCurrencies, valutes);

        // заполнить информационную строку
        RunTask2Info1.Text = $"Курс{(valutes.Count == 1 ? "" : "ы")} валют{(valutes.Count == 1 ? "ы" : "")} ";
        RunTask2InfoBold.Text = $"\"{valute}\"";
        RunTask2Info2.Text = ".";

        // заполнение строки состояния
        LblStatus.Content = $"Количество валют: {valutes.Count}.";

    } // ValutesByName_ClickAsync


    // сортировка курсов валют по убыванию
    private async void ValutesSortDescending_ClickAsync(object sender, RoutedEventArgs e) {

        // выборка данных по запросу
        var valutes = await _task2Controller.GetValutesSortDescending();

        // переход на вкладку
        TbcMain.SelectedIndex = 1;

        // заполнение таблицы
        UpdateDaraGrid(DgdCurrencies, valutes);

        // заполнить информационную строку
        RunTask2Info1.Text = "Курсы валют на ";
        RunTask2InfoBold.Text = $"\"{DateTime.Now:dd.MM.yyyyг. - HH:mm:ss}\"";
        RunTask2Info2.Text = ", упорядоченные по убыванию значений.";

        // заполнение строки состояния
        LblStatus.Content = $"Количество валют: {valutes.Count}.";

    } // ValutesSortDescending_ClickAsync

    #endregion


    #region Вспомогательные методы

    // обновление DataGrid
    public void UpdateDaraGrid<T>(DataGrid dgd, List<T> source) {

        dgd.ItemsSource = null;
        dgd.ItemsSource = source;

    } // UpdateDaraGrid

    #endregion

} // partial class MainWindow