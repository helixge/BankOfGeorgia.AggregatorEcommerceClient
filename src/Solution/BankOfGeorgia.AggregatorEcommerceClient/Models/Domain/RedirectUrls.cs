using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class RedirectUrls
{
    [JsonPropertyName("success")]
    public string? Success { get; set; }

    [JsonPropertyName("fail")]
    public string? Fail { get; set; }
}
