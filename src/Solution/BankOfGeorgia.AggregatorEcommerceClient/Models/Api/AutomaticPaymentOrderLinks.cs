using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class AutomaticPaymentOrderLinks
{
    [JsonPropertyName("details")]
    public ApiLinkDetails? Details { get; set; }
}
