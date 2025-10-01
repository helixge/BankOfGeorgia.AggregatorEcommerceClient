namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface ICallbackRequestVerificationService
{
    bool ValidateSignature(string requestBody, string signature);
}

internal class CallbackRequestVerificationService : ICallbackRequestVerificationService
{
    public bool ValidateSignature(string requestBody, string signature)
    {
        throw new CallbackValidationException($"Failed to validate signature", requestBody, signature);
    }
}
