using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Config
{
    [JsonPropertyName("loan")]
    public Loan? Loan { get; set; }

    [JsonPropertyName("campaign")]
    public Campaign? Campaign { get; set; }

    [JsonPropertyName("google_pay")]
    public GooglePay? GooglePay { get; set; }

    [JsonPropertyName("apple_pay")]
    public ApplePay? ApplePay { get; set; }

    [JsonPropertyName("account")]
    public Account? Account { get; set; }
}
