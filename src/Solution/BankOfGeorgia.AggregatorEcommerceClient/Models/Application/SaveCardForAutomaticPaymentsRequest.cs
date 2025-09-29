namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SaveCardForAutomaticPaymentsRequest
{
    public required string OrderId { get; init; }

    public Guid? IdempotencyKey { get; init; }
}
