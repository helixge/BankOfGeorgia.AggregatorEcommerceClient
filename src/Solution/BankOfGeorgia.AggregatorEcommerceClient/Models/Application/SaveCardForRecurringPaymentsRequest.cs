namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SaveCardForRecurringPaymentsRequest
{
    public required string OrderId { get; init; }

    public Guid? IdempotencyKey { get; init; }
}
