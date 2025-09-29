namespace BankOfGeorgia.AggregatorEcommerceClient;

public class RefundOrderRequest
{
    public required string OrderId { get; init; }

    public decimal? Amount { get; init; }

    public Guid? IdempotencyKey { get; init; }

    public RefundOrderAggregatorRequest ToRefundOrderAggregatorRequest() =>
        new()
        {
            Amount = Amount
        };
}
