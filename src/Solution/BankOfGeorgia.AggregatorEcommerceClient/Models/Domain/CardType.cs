using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum CardType
{
    [JsonPropertyName("")]
    Unknown = 0,

    [JsonPropertyName("visa")]
    Visa,

    [JsonPropertyName("mc")]
    MasterCard,

    [JsonPropertyName("amex")]
    AmericanExpress
}
