using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderPurchaseUnits
{
    [JsonPropertyName("request_amount")]
    public string? RequestAmount { get; set; }

    [JsonPropertyName("transfer_amount")]
    public string? TransferAmount { get; set; }

    [JsonPropertyName("refund_amount")]
    public string? RefundAmount { get; set; }

    [JsonPropertyName("currency_code")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("items")]
    public IEnumerable<OrderBasketItem>? Items { get; set; }
}
