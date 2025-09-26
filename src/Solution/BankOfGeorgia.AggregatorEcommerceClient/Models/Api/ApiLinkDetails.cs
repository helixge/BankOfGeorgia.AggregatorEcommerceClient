using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class ApiLinkDetails
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
