using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace NetProgTask1Task2.Models;

// Ежедневные курсы валют, полученные через API ЦРБ
public class DailyCurrency
{
    // [JsonProperty("Date")]
    public DateTime Date { get; set; }

    // [JsonProperty("PreviousDate")]
    public DateTime PreviousDate { get; set; }

    [JsonProperty("PreviousURL")]
    public string PreviousUrl { get; set; } = string.Empty;

    // [JsonProperty("Timestamp")]
    public DateTime Timestamp { get; set; }

    // Атрибут для согласования имени свойства
    // в файле JSON с именем  в классе
    [JsonProperty("Valute")]
    public Dictionary<string, Valute> Valutes { get; set; } = new();


    #region Работа с JSON: десериализация и сериализация

    public static DailyCurrency FromJson(string json) =>
        JsonConvert.DeserializeObject<DailyCurrency>(json)!;

    public static string ToJson(DailyCurrency dailyCurrency) =>
        JsonConvert.SerializeObject(dailyCurrency, Formatting.Indented)!;

    #endregion

} // class DailyCurrency