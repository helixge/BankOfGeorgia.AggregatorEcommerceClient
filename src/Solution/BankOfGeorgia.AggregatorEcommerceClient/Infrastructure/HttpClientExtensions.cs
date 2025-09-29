namespace BankOfGeorgia.AggregatorEcommerceClient;

public static class HttpClientExtensions
{
    public static async Task<TResponse> MakeBankOfGeorgiaRequest<TResponse>(this HttpClient httpClient, HttpRequestMessage request, IBankOfGeorgiaApiSerializationService serializer)
    {
        HttpResponseMessage response = await httpClient.SendAsync(request);
        string responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new BankOfGeorgiaApiException(responseContent, ex);
        }

        if (string.IsNullOrWhiteSpace(responseContent))
        {
            throw new BankOfGeorgiaApiException("HTTP response was empty");
        }

        TResponse? responseData = serializer.Deserialize<TResponse>(responseContent);

        if (responseData is null)
        {
            throw new BankOfGeorgiaApiException($"Failed to deserialize HTTP response: '{responseContent}'");
        }

        return responseData;
    }

    public static async Task MakeBankOfGeorgiaRequestExpectingAccepted(this HttpClient httpClient, HttpRequestMessage request, string operationName)
    {
        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
        {
            return;
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        throw new BankOfGeorgiaApiException($"{operationName} failed with status {response.StatusCode}", responseContent);
    }
}
