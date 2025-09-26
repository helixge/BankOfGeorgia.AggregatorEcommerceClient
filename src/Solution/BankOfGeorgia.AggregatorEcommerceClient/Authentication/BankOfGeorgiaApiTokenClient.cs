using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaApiTokenClient
{
    Task<TokenApiResponse> GetToken();
}

internal class BankOfGeorgiaApiTokenClient(
    IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions> options,
    HttpClient httpClient
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

        HttpResponseMessage response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        TokenApiResponse? responseData = await response.Content.ReadFromJsonAsync<TokenApiResponse>();

        if (responseData is null)
        {
            throw new BankOfGeorgiaApiTokenException("Failed to deserialize token response or response was empty");
        }

        if (string.IsNullOrWhiteSpace(responseData.AccessToken))
        {
            throw new BankOfGeorgiaApiTokenException("Access token is missing in the response");
        }

        if (string.Equals(responseData.TokenType, "bearer", StringComparison.OrdinalIgnoreCase) is false)
        {
            throw new BankOfGeorgiaApiTokenException($"Unexpected token type: {responseData.TokenType}");
        }

        return responseData;
    }
}
