namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaApiException : Exception
{
    public BankOfGeorgiaApiException(string message)
        : base(message)
    {
    }

    public BankOfGeorgiaApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
