using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationType
{
    [JsonPropertyName("web")]
    Web,
    [JsonPropertyName("mobile")]
    Mobile
}
