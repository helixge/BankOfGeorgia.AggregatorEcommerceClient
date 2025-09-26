using System.Net.Http.Headers;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaAggregatorEcommerceClient
{
    Task<SubmitOrderResponse> SubmitOrder(SubmitOrderRequest request);
}

internal class BankOfGeorgiaAggregatorEcommerceClient(
    IBankOfGeorgiaApiTokenClient bankOfGeorgiaApiTokenClient,
    IBankOfGeorgiaApiSerializationService serializer,
    HttpClient httpClient
) : IBankOfGeorgiaAggregatorEcommerceClient
{
    public async Task<SubmitOrderResponse> SubmitOrder(SubmitOrderRequest request)
    {
        TokenApiResponse token = await bankOfGeorgiaApiTokenClient.GetToken();
        HttpRequestMessage requestMessage = new(HttpMethod.Post, "v1/ecommerce/orders");
        AuthenticationHeaderValue authHeader = new("Bearer", token.AccessToken);

        requestMessage.Headers.Authorization = authHeader;

        if (request.IdempotencyKey is not null)
        {
            requestMessage.Headers.Add("Idempotency-Key", serializer.Serialize(request.IdempotencyKey));
        }

        if (request.Language is not null)
        {
            requestMessage.Headers.Add("Accept-Language", serializer.Serialize(request.Language));
        }

        if (request.Theme is not null)
        {
            requestMessage.Headers.Add("Theme", serializer.Serialize(request.Theme));
        }

        SubmitOrderAggregatorRequest aggregatorRequest = request.ToSubmitOrderRequest();
        string serializedContent = serializer.Serialize(aggregatorRequest);
        StringContent requestContent = new(serializedContent, Encoding.UTF8, "application/json");
        requestMessage.Content = requestContent;

        SubmitOrderAggregatorResponse aggregatorResponse = await httpClient.MakeBankOfGeorgiaRequest<SubmitOrderAggregatorResponse>(requestMessage, serializer);
        SubmitOrderResponse response = aggregatorResponse.ToSubmitOrderResponse();

        if (string.IsNullOrWhiteSpace(response.Id))
        {
            throw new BankOfGeorgiaApiException($"{nameof(SubmitOrder)} resulted in an empty {nameof(response.Id)}");
        }

        return response;
    }


}
