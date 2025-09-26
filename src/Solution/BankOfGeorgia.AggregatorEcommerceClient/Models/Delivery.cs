using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Delivery
{
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }
}
