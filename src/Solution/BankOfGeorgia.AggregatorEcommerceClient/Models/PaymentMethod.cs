using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMethod
{
    [JsonPropertyName("card")]
    Card,
    [JsonPropertyName("google_pay")]
    GooglePay,
    [JsonPropertyName("apple_pay")]
    ApplePay,
    [JsonPropertyName("bog_p2p")]
    BogP2P,
    [JsonPropertyName("bog_loyalty")]
    BogLoyalty,
    [JsonPropertyName("bnpl")]
    Bnpl,
    [JsonPropertyName("bog_loan")]
    BogLoan,
    [JsonPropertyName("gift_card")]
    GiftCard
}
