using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class PaymentOrderLinks
{
    [JsonPropertyName("details")]
    public ApiLinkDetails? Details { get; set; }

    [JsonPropertyName("redirect")]
    public ApiLinkDetails? Redirect { get; set; }
}
