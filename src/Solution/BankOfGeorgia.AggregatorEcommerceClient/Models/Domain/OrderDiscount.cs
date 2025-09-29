using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderDiscount
{
    [JsonPropertyName("bank_discount_amount")]
    public string? BankDiscountAmount { get; set; }

    [JsonPropertyName("bank_discount_desc")]
    public string? BankDiscountDesc { get; set; }

    [JsonPropertyName("discounted_amount")]
    public string? DiscountedAmount { get; set; }

    [JsonPropertyName("original_order_amount")]
    public string? OriginalOrderAmount { get; set; }

    [JsonPropertyName("system_discount_amount")]
    public string? SystemDiscountAmount { get; set; }

    [JsonPropertyName("system_discount_desc")]
    public string? SystemDiscountDesc { get; set; }
}
