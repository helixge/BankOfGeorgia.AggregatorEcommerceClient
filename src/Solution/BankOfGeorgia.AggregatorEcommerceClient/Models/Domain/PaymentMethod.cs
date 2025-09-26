using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum PaymentMethod
{
    /// <summary>
    /// Payment by a bank card
    /// </summary>
    [JsonPropertyName("card")]
    Card,

    /// <summary>
    /// Payment through Google Pay and by a bank card (in the case of providing this option, the customer will be able to pay both by Google Pay and a bank card. The Business must have both payment methods activated)
    /// </summary>
    [JsonPropertyName("google_pay")]
    GooglePay,

    /// <summary>
    /// Payment through Apple Pay and by a bank card (in the case of providing this option, the customer will be able to pay both by Apple Pay and a bank card. The Business must have both payment methods activated)
    /// </summary>
    [JsonPropertyName("apple_pay")]
    ApplePay,

    /// <summary>
    /// Transferring by the BoG, internet, or mobile bank user
    /// </summary>
    [JsonPropertyName("bog_p2p")]
    BogP2P,

    /// <summary>
    /// Payment by the BoG MR/Plus points
    /// </summary>
    [JsonPropertyName("bog_loyalty")]
    BogLoyalty,

    /// <summary>
    /// Payment with Buy Now Pay Later plan
    /// </summary>
    [JsonPropertyName("bnpl")]
    Bnpl,

    /// <summary>
    /// Standard bank installment plan
    /// </summary>
    [JsonPropertyName("bog_loan")]
    BogLoan,

    /// <summary>
    /// Payment with a gift card
    /// </summary>
    [JsonPropertyName("gift_card")]
    GiftCard
}
