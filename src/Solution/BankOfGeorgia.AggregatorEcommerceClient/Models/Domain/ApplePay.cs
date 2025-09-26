using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class ApplePay
{
    [JsonPropertyName("external")]
    public bool? External { get; set; }
}
