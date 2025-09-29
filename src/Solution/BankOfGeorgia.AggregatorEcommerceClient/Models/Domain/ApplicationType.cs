using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum ApplicationType
{
    [JsonPropertyName("")]
    Unknown = 0,

    [JsonPropertyName("web")]
    Web,

    [JsonPropertyName("mobile")]
    Mobile
}
