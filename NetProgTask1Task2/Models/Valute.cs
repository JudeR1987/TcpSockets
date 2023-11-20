using Newtonsoft.Json;

namespace NetProgTask1Task2.Models;

// Описание валюты в файле JSON, полученном через API ЦРБ
public class Valute
{
    // атрибут для согласования имени свойства
    // в файле JSON с именем свойства в классе
    [JsonProperty("ID")]
    public string Id { get; set; } = string.Empty;

    // [JsonProperty("NumCode")]
    public string NumCode { get; set; } = string.Empty;

    // [JsonProperty("CharCode")]
    public string CharCode { get; set; } = string.Empty;

    // [JsonProperty("Nominal")]
    public long Nominal { get; set; }

    // [JsonProperty("Name")]
    public string Name { get; set; } = string.Empty;

    // [JsonProperty("Value")]
    public double Value { get; set; }

    // [JsonProperty("Previous")]
    public double Previous { get; set; }

} // class Valute