using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Campaign
{
    [JsonPropertyName("card")]
    public CardType? Card { get; set; }

    [JsonPropertyName("type")]
    public CampaignType? Type { get; set; }
}
