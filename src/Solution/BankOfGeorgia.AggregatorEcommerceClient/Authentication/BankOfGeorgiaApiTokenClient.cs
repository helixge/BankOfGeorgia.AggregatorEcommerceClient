using Microsoft.Extensions.Options;

namespace BankOfGeorgia.AggregatorEcommerceClient;

internal class BankOfGeorgiaApiTokenClient(
    IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions> options,
    HttpClient httpClient,
    IBankOfGeorgiaApiSerializationService serializer
    ) : IBankOfGeorgiaApiTokenClient
{
    public async Task<TokenApiResponse> GetToken()
    {
        string url = options.Value.OAuthUrlOrDefault();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

        FormUrlEncodedContent formContent = new(
        [
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        ]);
        request.Content = formContent;

        TokenApiResponse response = await httpClient.MakeBankOfGeorgiaRequest<TokenApiResponse>(request, serializer);

        if (string.IsNullOrWhiteSpace(response.AccessToken))
        {
            throw new BankOfGeorgiaApiException("Access token is missing in the response");
        }

        if (string.Equals(response.TokenType, "bearer", StringComparison.OrdinalIgnoreCase) is false)
        {
            throw new BankOfGeorgiaApiException($"Unexpected token type: {response.TokenType}");
        }

        return response;
    }
}
