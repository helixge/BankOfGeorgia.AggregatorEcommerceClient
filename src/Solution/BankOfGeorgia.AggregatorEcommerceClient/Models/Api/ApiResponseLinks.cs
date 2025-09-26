using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class ApiResponseLinks
{
    [JsonPropertyName("details")]
    public ApiLinkDetails? Details { get; set; }

    [JsonPropertyName("redirect")]
    public ApiLinkDetails? Redirect { get; set; }
}
