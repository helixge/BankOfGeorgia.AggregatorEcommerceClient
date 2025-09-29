using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderBasketItem
{
    [JsonPropertyName("external_item_id")]
    public string? ExternalItemId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("unit_price")]
    public string? UnitPrice { get; set; }

    [JsonPropertyName("unit_discount_price")]
    public string? UnitDiscountPrice { get; set; }

    [JsonPropertyName("vat")]
    public string? Vat { get; set; }

    [JsonPropertyName("vat_percent")]
    public string? VatPercent { get; set; }

    [JsonPropertyName("total_price")]
    public string? TotalPrice { get; set; }

    [JsonPropertyName("package_code")]
    public string? PackageCode { get; set; }

    [JsonPropertyName("tin")]
    public string? Tin { get; set; }

    [JsonPropertyName("pinfl")]
    public string? Pinfl { get; set; }

    [JsonPropertyName("product_discount_id")]
    public string? ProductDiscountId { get; set; }
}
