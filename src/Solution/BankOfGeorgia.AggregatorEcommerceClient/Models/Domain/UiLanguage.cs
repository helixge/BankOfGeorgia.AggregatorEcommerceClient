using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum UiLanguage
{
    [JsonPropertyName("ka")]
    Georgian = 0,

    [JsonPropertyName("en")]
    English = 1,
}
