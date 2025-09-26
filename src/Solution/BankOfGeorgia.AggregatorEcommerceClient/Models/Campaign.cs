using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Campaign
{
    [JsonPropertyName("card")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CardType? Card { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CampaignType? Type { get; set; }
}
