namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitSubscriptionPaymentOrderRequest
{
    public required string ParentOrderId { get; init; }

    public string? CallbackUrl { get; init; }

    public string? ExternalOrderId { get; init; }

    public Guid? IdempotencyKey { get; init; }

    public SubmitSubscriptionPaymentOrderAggregatorRequest ToSubmitSubscriptionPaymentOrderAggregatorRequest() =>
        new()
        {
            CallbackUrl = CallbackUrl,
            ExternalOrderId = ExternalOrderId
        };
}
