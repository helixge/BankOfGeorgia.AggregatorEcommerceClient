namespace BankOfGeorgia.AggregatorEcommerceClient;

public class DeleteSavedCardRequest
{
    public required string OrderId { get; init; }

    public Guid? IdempotencyKey { get; init; }
}
