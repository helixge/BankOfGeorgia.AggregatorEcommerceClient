using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderStatus
{
    [JsonPropertyName("key")]
    public OrderStatusType? Key { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
