using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class TransferMethod
{
    [JsonPropertyName("key")]
    public PaymentMethod? Key { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
