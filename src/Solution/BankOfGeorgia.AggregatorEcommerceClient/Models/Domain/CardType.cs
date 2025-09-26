using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardType
{
    [JsonPropertyName("visa")]
    Visa,

    [JsonPropertyName("mc")]
    MasterCard,

    [JsonPropertyName("solo")]
    Solo
}
