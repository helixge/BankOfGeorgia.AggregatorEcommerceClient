using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class OrderClient
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("brand_ka")]
    public string? BrandKa { get; set; }

    [JsonPropertyName("brand_en")]
    public string? BrandEn { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
