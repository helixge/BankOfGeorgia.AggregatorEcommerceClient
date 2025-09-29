using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class GetOrderDetailsAggregatorResponse
{
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }

    [JsonPropertyName("industry")]
    public string? Industry { get; init; }

    [JsonPropertyName("capture")]
    public CaptureType? Capture { get; init; }

    [JsonPropertyName("external_order_id")]
    public string? ExternalOrderId { get; init; }

    [JsonPropertyName("client")]
    public OrderClient? Client { get; init; }

    [JsonPropertyName("zoned_create_date")]
    public string? ZonedCreateDate { get; init; }

    [JsonPropertyName("zoned_expire_date")]
    public string? ZonedExpireDate { get; init; }

    [JsonPropertyName("order_status")]
    public OrderStatus? OrderStatus { get; init; }

    [JsonPropertyName("buyer")]
    public OrderBuyer? Buyer { get; init; }

    [JsonPropertyName("purchase_units")]
    public OrderPurchaseUnits? PurchaseUnits { get; init; }

    [JsonPropertyName("redirect_links")]
    public OrderRedirectLinks? RedirectLinks { get; init; }

    [JsonPropertyName("payment_detail")]
    public PaymentDetail? PaymentDetail { get; init; }

    [JsonPropertyName("discount")]
    public OrderDiscount? Discount { get; init; }

    [JsonPropertyName("actions")]
    public IEnumerable<OrderAction>? Actions { get; init; }

    [JsonPropertyName("lang")]
    public UiLanguage? Lang { get; init; }

    [JsonPropertyName("reject_reason")]
    public string? RejectReason { get; init; }

    internal GetOrderDetailsResponse ToGetOrderDetailsResponse()
        => new()
        {
            OrderId = OrderId,
            Capture = Capture,
            ExternalOrderId = ExternalOrderId,
            OrderStatus = OrderStatus,
            PaymentDetail = PaymentDetail
        };
}
