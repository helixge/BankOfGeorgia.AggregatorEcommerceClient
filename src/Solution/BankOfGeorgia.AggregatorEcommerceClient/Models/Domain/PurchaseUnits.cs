using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class PurchaseUnits
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "GEL";

    [JsonPropertyName("total_amount")]
    public required decimal TotalAmount { get; set; }

    [JsonPropertyName("basket")]
    public required IEnumerable<BasketItem> Basket { get; set; }

    [JsonPropertyName("delivery")]
    public Delivery? Delivery { get; set; }

    [JsonPropertyName("total_discount_amount")]
    public decimal? TotalDiscountAmount { get; set; }
}
