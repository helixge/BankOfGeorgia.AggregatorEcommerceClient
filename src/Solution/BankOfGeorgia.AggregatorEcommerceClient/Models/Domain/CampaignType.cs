using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CampaignType
{
    [JsonPropertyName("restrict")]
    Restrict,

    [JsonPropertyName("client_discount")]
    ClientDiscount
}
