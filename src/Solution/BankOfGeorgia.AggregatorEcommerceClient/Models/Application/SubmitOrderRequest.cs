namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitOrderRequest
{
    public Guid? IdempotencyKey { get; init; }

    public UiLanguage? Language { get; init; }

    public UiTheme? Theme { get; init; }

    public ApplicationType? ApplicationType { get; init; }

    public Buyer? Buyer { get; init; }

    public required string CallbackUrl { get; init; }

    public string? ExternalOrderId { get; init; }

    public required PurchaseUnits PurchaseUnits { get; init; }

    public RedirectUrls? RedirectUrls { get; init; }

    public int? Ttl { get; init; }

    public required IEnumerable<PaymentMethod> PaymentMethod { get; init; }

    public Config? Config { get; init; }

    public CaptureType? Capture { get; init; }

    public SubmitOrderAggregatorRequest ToSubmitOrderAggregatorRequest() =>
        new()
        {
            ApplicationType = ApplicationType,
            Buyer = Buyer,
            CallbackUrl = CallbackUrl,
            ExternalOrderId = ExternalOrderId,
            PurchaseUnits = PurchaseUnits,
            RedirectUrls = RedirectUrls,
            Capture = Capture,
            Ttl = Ttl,
            PaymentMethod = PaymentMethod,
            Config = Config
        };
}
