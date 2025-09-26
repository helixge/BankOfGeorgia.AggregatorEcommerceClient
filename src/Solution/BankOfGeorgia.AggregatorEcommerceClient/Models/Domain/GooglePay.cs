using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class GooglePay
{
    [JsonPropertyName("google_pay_token")]
    public string? GooglePayToken { get; set; }

    [JsonPropertyName("external")]
    public bool? External { get; set; }
}
