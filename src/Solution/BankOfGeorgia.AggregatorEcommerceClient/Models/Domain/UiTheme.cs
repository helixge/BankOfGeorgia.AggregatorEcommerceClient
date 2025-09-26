using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum UiTheme
{
    [JsonPropertyName("light")]
    Light = 0,

    [JsonPropertyName("dark")]
    Dark = 1,
}
