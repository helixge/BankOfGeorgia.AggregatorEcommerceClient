using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public enum OrderStatusType
{
    [JsonPropertyName("")]
    Unknown = 0,

    /// <summary>
    /// payment request is created
    /// </summary>
    [JsonPropertyName("created")]
    Created,

    /// <summary>
    /// payment is being processed
    /// </summary>
    [JsonPropertyName("processing")]
    Processing,

    /// <summary>
    /// payment process has been completed
    /// </summary>
    [JsonPropertyName("completed")]
    Completed,

    /// <summary>
    /// payment process has been unsuccessfully completed
    /// </summary>
    [JsonPropertyName("rejected")]
    Rejected,

    /// <summary>
    /// refund of the amount is requested
    /// </summary>
    [JsonPropertyName("refund_requested")]
    RefundRequested,

    /// <summary>
    /// payment amount has been returned
    /// </summary>
    [JsonPropertyName("refunded")]
    Refunded,

    /// <summary>
    /// payment amount has been partially refunded
    /// </summary>
    [JsonPropertyName("refunded_partially")]
    RefundedPartially,

    /// <summary>
    /// pre-authorize payment is requested
    /// </summary>
    [JsonPropertyName("auth_requested")]
    AuthRequested,

    /// <summary>
    /// pre-authorize payment has been completed successfully, but payment amount is blocked and waiting for confirmation
    /// </summary>
    [JsonPropertyName("blocked")]
    Blocked,

    /// <summary>
    /// pre-authorize payment partial amount has been confirmed successfully
    /// </summary>
    [JsonPropertyName("partial_completed")]
    PartialCompleted
}
