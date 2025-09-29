namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaApiException : Exception
{
    public object? DetailsObject { get; init; }

    public BankOfGeorgiaApiException(string message)
        : base(message)
    {
    }

    public BankOfGeorgiaApiException(string message, object? detailsObject)
        : base(message)
    {
        DetailsObject = detailsObject;
    }

    public BankOfGeorgiaApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
