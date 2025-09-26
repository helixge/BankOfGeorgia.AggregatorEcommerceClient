namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaApiTokenException : Exception
{
    public BankOfGeorgiaApiTokenException(string message)
        : base(message)
    {
    }
}
