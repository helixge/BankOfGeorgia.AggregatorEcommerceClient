namespace BankOfGeorgia.AggregatorEcommerceClient;

public class CallbackValidationException : Exception
{
    public string? RequestBody { get; init; }
    public string? Signature { get; init; }

    public CallbackValidationException(string message)
        : base(message)
    {

    }

    public CallbackValidationException(string message, string requestBody)
        : base(message)
    {
        RequestBody = requestBody;
    }

    public CallbackValidationException(string message, string requestBody, string signature)
        : base(message)
    {
        RequestBody = requestBody;
        Signature = signature;
    }

    public CallbackValidationException(string message, string requestBody, Exception innerException)
        : base(message, innerException)
    {
        RequestBody = requestBody;
    }

    public CallbackValidationException(string message, string requestBody, string signature, Exception innerException)
       : base(message, innerException)
    {
        RequestBody = requestBody;
        Signature = signature;
    }
}
