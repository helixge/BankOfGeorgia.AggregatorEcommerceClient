using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class RefundOrderAggregatorRequest
{
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }
}
