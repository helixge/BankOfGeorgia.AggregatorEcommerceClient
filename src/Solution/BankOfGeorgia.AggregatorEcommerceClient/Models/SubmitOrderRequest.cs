using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitOrderRequest
{
    [JsonPropertyName("callback_url")]
    public required string CallbackUrl { get; set; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; set; }

    [JsonPropertyName("purchase_units")]
    public required PurchaseUnits PurchaseUnits { get; set; }

    [JsonPropertyName("redirect_urls")]
    public RedirectUrls? RedirectUrls { get; set; }

    [JsonPropertyName("application_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApplicationType? ApplicationType { get; set; }

    [JsonPropertyName("buyer")]
    public Buyer? Buyer { get; set; }

    [JsonPropertyName("capture")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CaptureType? Capture { get; set; }

    [JsonPropertyName("ttl")]
    public int? Ttl { get; set; }

    [JsonPropertyName("payment_method")]
    public List<PaymentMethod>? PaymentMethod { get; set; }

    [JsonPropertyName("config")]
    public Config? Config { get; set; }
}
