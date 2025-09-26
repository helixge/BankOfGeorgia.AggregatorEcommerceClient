using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BasketItem
{
    [JsonPropertyName("product_id")]
    public required string ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }

    [JsonPropertyName("unit_price")]
    public required decimal UnitPrice { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("unit_discount_price")]
    public decimal? UnitDiscountPrice { get; set; }

    [JsonPropertyName("vat")]
    public decimal? Vat { get; set; }

    [JsonPropertyName("vat_percent")]
    public decimal? VatPercent { get; set; }

    [JsonPropertyName("total_price")]
    public decimal? TotalPrice { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("package_code")]
    public string? PackageCode { get; set; }

    [JsonPropertyName("tin")]
    public string? Tin { get; set; }

    [JsonPropertyName("pinfl")]
    public string? Pinfl { get; set; }

    [JsonPropertyName("product_discount_id")]
    public string? ProductDiscountId { get; set; }
}
