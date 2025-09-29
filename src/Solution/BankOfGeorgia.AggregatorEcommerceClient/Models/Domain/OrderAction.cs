using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderAction
{
    [JsonPropertyName("action_id")]
    public string? ActionId { get; set; }

    [JsonPropertyName("request_channel")]
    public string? RequestChannel { get; set; }

    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("zoned_action_date")]
    public string? ZonedActionDate { get; set; }

    [JsonPropertyName("amount")]
    public string? Amount { get; set; }
}
