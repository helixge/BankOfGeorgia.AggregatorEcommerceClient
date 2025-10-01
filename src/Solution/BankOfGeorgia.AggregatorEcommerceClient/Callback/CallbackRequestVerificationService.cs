using System.Security.Cryptography;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface ICallbackRequestVerificationService
{
    void ValidateSignature(Span<byte> requestBody, Span<byte> signature);
}

internal class CallbackRequestVerificationService : ICallbackRequestVerificationService
{
    public void ValidateSignature(Span<byte> requestBody, Span<byte> signature)
    {
        if (requestBody == null || requestBody.Length == 0)
        {
            throw new CallbackValidationException("Request body is empty");
        }

        if (signature == null || signature.Length == 0)
        {
            throw new CallbackValidationException("Signature is empty");
        }

        try
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(Constants.CallbackPublicKey);

            bool isValid = rsa.VerifyData(
                requestBody,
                signature,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1
            );

            if (!isValid)
            {
                string bodyString = Encoding.UTF8.GetString(requestBody);
                string signatureString = Convert.ToBase64String(signature);
                throw new CallbackValidationException("Signature verification failed", bodyString, signatureString);
            }
        }
        catch (CryptographicException ex)
        {
            string bodyString = Encoding.UTF8.GetString(requestBody);
            string signatureString = Convert.ToBase64String(signature);
            throw new CallbackValidationException("Cryptographic error during signature verification", bodyString, signatureString, ex);
        }
    }
}
