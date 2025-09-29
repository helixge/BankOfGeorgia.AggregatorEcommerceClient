using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum CampaignType
{
    [JsonPropertyName("")]
    Unknown = 0,

    [JsonPropertyName("restrict")]
    Restrict,

    [JsonPropertyName("client_discount")]
    ClientDiscount
}
