namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitOrderRequest
{
    public Guid? IdempotencyKey { get; set; }

    public UiLanguage? Language { get; set; }

    public UiTheme? Theme { get; set; }

    public ApplicationType? ApplicationType { get; set; }

    public Buyer? Buyer { get; internal set; }

    public required string CallbackUrl { get; set; }

    public string? ExternalOrderId { get; set; }

    public required PurchaseUnits PurchaseUnits { get; set; }

    public RedirectUrls? RedirectUrls { get; set; }

    public int? Ttl { get; set; }

    public required IEnumerable<PaymentMethod> PaymentMethod { get; set; }

    public Config? Config { get; set; }

    public CaptureType? Capture { get; set; }

    public SubmitOrderAggregatorRequest ToSubmitOrderRequest() =>
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
