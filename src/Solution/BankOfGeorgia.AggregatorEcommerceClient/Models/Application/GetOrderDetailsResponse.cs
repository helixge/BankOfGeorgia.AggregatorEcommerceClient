namespace BankOfGeorgia.AggregatorEcommerceClient;

public class GetOrderDetailsResponse
{
    public string? OrderId { get; init; }
    public CaptureType? Capture { get; init; }
    public string? ExternalOrderId { get; init; }
    public OrderStatus? OrderStatus { get; init; }
    public PaymentDetail? PaymentDetail { get; init; }
}
