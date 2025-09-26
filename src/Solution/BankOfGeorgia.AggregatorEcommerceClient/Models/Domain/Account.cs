using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Account
{
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }
}
