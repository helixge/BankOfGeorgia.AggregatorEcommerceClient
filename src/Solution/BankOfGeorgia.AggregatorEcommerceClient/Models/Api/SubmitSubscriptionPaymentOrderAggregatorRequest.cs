using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitSubscriptionPaymentOrderAggregatorRequest
{
    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; set; }
}
