using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Loan
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("month")]
    public int? Month { get; set; }
}
