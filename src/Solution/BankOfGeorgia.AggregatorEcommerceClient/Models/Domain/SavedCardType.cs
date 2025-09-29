using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum SavedCardType
{
    [JsonPropertyName("")]
    Unknown = 0,

    [JsonPropertyName("recurrent")]
    Recurrent,

    [JsonPropertyName("subscription")]
    Subscription
}
