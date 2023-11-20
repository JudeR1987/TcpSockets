using NetProgTask1Task2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetProgTask1Task2.Controllers;

// Контроллер для выполнения обработок по задаче 2
public class Task2Controller
{
    // адрес получения данных о курсах валют
    public readonly string Uri = "https://www.cbr-xml-daily.ru/daily_json.js";


    // коллекция валют
    public List<Valute> Valutes = new();


    // справочник код валюты -- страна, название валюты
    public List<CountryCode> CountryCodes;


    // Конструктор по умолчанию - создание коллекций, заполнение справочника 
    public Task2Controller() {

        // хардкодим имя папки, надо руками создавать папку, копировать файл
        CountryCodes = CountryCode.FromJson(File.ReadAllText(@"App_Files\codes.json"));

        // GetValutesAsync().Wait();                                                // так виснет даже в отладке
        //Task.Factory.StartNew(async () => await GetValutesAsync()).Wait();        // почему данный код не работает ???!!!
        Task.Run(GetValutesAsync).Wait();                                           // а этот работает               ???!!!


    } // Task2Controller


    // Получение курсов валют 
    public async Task GetValutesAsync() {

        var request = (HttpWebRequest)WebRequest.Create(Uri);
        var response =  (HttpWebResponse)(await request.GetResponseAsync());

        using var srd = new StreamReader(response.GetResponseStream());
        var sb = new StringBuilder();
        
        while (true) {

            var str = await srd.ReadLineAsync()!;
            if (str == null) break;

            sb.AppendLine(str);

        } // while

        response.Close();

        // Записать полученные данные из API ЦРБ в файл
        var parts = Uri.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var fileName = "App_Files\\" + parts[parts!.Length - 1];

        await File.WriteAllTextAsync(fileName, sb.ToString(), Encoding.UTF8);

        // Сформировать коллекцию данных о курсах валют
        Valutes = DailyCurrency.FromJson(sb.ToString()).Valutes
            .Select(v => v.Value)
            .OrderBy(valute => valute.Name)
            .ToList();

    } // GetValutesAsync


    #region Получение данных по заданию

    // Найти валюты с курсом в заданном диапазоне значений
    public Task<List<Valute>> ValutesInRange(double loValue, double hiValue) {

        return Task.Run(() => {

            var valutes = Valutes
            .Where(valute => loValue <= valute.Value && valute.Value <= hiValue)
            .OrderBy(valute => valute.Value)
            .ToList();

            return valutes;
        });

    } // ValutesInRange


    // Найти курс валюты заданной страны
    public Task<List<Valute>> ValuteByCountry(string country) {

        return Task.Run(() => {

            var valutes = Valutes
            .Join(
                CountryCodes,
                valute => valute.CharCode,
                countryCode => countryCode.CharCode,
                (valute, countryCode) => new {
                    Country = countryCode.Country,
                    Valute = valute
                })
            .Where(cv => cv.Country == country)
            .Select(cv => cv.Valute)
            .ToList();

            return valutes;
        });

    } // ValuteByCountry


    // Найти курс заданной по наименованию валюты 
    public Task<List<Valute>> ValuteByName(string valuteName) {

        return Task.Run(() => {

            var valutes = Valutes
            .Join(
                CountryCodes,
                valute => valute.CharCode,
                countryCode => countryCode.CharCode,
                (valute, countryCode) => new {
                    Valute = valute,
                    Info = countryCode
                })
            .Where(vi => vi.Info.Name.ToLower().Contains(valuteName.ToLower()))
            .Select(vi => vi.Valute)
            .Distinct()
            .ToList();

            return valutes;
        });

    } // ValuteByName


    // Упорядочить курсы валют по убыванию
    public Task<List<Valute>> GetValutesSortDescending() {

        return Task.Run(() => {

            var valutes = Valutes
            .OrderByDescending(valute => valute.Value)
            .ToList();

            return valutes;
        });

    } // GetValutesSortDescending

    #endregion


    #region Вспомогательные методы

    // выборка значений всех доступных курсов валют
    public Task<List<double>> GetAllValutesValues() {

        return Task.Run(() => {

            var allValutesValues = Valutes
            .Select(valute => valute.Value)
            .Distinct()
            .Order()
            .ToList();

            return allValutesValues;
        });

    } // GetAllValutesValues


    // выборка стран всех доступных курсов валют
    public Task<List<string>> GetAllCountries() {

        return Task.Run(() => {

            var countries = Valutes
            .Join(
                CountryCodes,
                valute => valute.CharCode,
                countryCode => countryCode.CharCode,
                (valute, countryCode) => new {
                    Country = countryCode.Country,
                    Valute = valute
                })
            .Select(cv => cv.Country)
            .Order()
            .ToList();

            return countries;
        });

    } // GetAllCountries


    // выборка наименований всех доступных курсов валют
    public Task<List<string>> GetAllValutes() {

        return Task.Run(() => {

            var valutesNames = Valutes
            .Join(
                CountryCodes,
                valute => valute.CharCode,
                countryCode => countryCode.CharCode,
                (valute, countryCode) => new {
                    Valute = valute,
                    Info = countryCode
                })
            .Select(vi => vi.Info.Name)
            .Distinct()
            .ToList();

            return valutesNames;
        });

    } // GetAllValutes

    #endregion

} // class Task2Controller