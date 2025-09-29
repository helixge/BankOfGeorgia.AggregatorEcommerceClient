using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class RefundOrderAggregatorResponse
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("action_id")]
    public string? ActionId { get; set; }

    public RefundOrderResponse ToRefundOrderResponse()
        => new()
        {
            Key = Key,
            Message = Message,
            ActionId = ActionId
        };
}
