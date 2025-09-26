using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaptureType
{
    [JsonPropertyName("automatic")]
    Automatic,
    [JsonPropertyName("manual")]
    Manual
}
