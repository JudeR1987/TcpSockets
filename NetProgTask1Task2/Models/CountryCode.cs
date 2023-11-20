using Newtonsoft.Json;
using System.Collections.Generic;

namespace NetProgTask1Task2.Models;

// Коды валют для государства
// ЦРБ
// http://www.cbr.ru/scripts/XML_val.asp?d=0
public class CountryCode
{
    [JsonProperty("col1")]
    public string Country { get; set; } = string.Empty;

    [JsonProperty("col2")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("col3")]
    public string CharCode { get; set; } = string.Empty;


    #region Работа с JSON: десериализация и сериализация

    public static List<CountryCode> FromJson(string json) =>
        JsonConvert.DeserializeObject<List<CountryCode>>(json)!;

    public static string ToJson(List<CountryCode> countryCodes) =>
        JsonConvert.SerializeObject(countryCodes, Formatting.Indented)!;

    #endregion

} // class CountryCode