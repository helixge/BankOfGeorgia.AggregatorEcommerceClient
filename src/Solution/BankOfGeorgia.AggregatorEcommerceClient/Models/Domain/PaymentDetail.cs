using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class PaymentDetail
{
    [JsonPropertyName("transfer_method")]
    public TransferMethod? TransferMethod { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("code_description")]
    public string? CodeDescription { get; set; }

    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("payer_identifier")]
    public string? PayerIdentifier { get; set; }

    [JsonPropertyName("payment_option")]
    public string? PaymentOption { get; set; }

    [JsonPropertyName("card_type")]
    public CardType? CardType { get; set; }

    [JsonPropertyName("card_expiry_date")]
    public string? CardExpiryDate { get; set; }

    [JsonPropertyName("request_account_tag")]
    public string? RequestAccountTag { get; set; }

    [JsonPropertyName("transfer_account_tag")]
    public string? TransferAccountTag { get; set; }

    [JsonPropertyName("saved_card_type")]
    public SavedCardType? SavedCardType { get; set; }

    [JsonPropertyName("parent_order_id")]
    public string? ParentOrderId { get; set; }

    [JsonPropertyName("auth_code")]
    public string? AuthCode { get; set; }
}
