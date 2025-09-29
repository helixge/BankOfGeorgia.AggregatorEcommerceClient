using System.Net.Http.Headers;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaAggregatorEcommerceClient
{
    Task<SubmitOrderResponse> SubmitOrder(SubmitOrderRequest request);
    Task<GetOrderDetailsResponse> GetOrderDetails(GetOrderDetailsRequest request);
    Task<SaveCardForRecurringPaymentsResponse> SaveCardForRecurringPayments(SaveCardForRecurringPaymentsRequest request);
    Task<SaveCardForAutomaticPaymentsResponse> SaveCardForAutomaticPayments(SaveCardForAutomaticPaymentsRequest request);
    Task<DeleteSavedCardResponse> DeleteSavedCard(DeleteSavedCardRequest request);
}

internal class BankOfGeorgiaAggregatorEcommerceClient(
    IBankOfGeorgiaApiTokenClient bankOfGeorgiaApiTokenClient,
    IBankOfGeorgiaApiSerializationService serializer,
    HttpClient httpClient
) : IBankOfGeorgiaAggregatorEcommerceClient
{
    public async Task<SubmitOrderResponse> SubmitOrder(SubmitOrderRequest request)
    {
        string url = "v1/ecommerce/orders";
        HttpRequestMessage requestMessage = await CreateAuthenticatedRequestMessage(HttpMethod.Post, url);

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

        var aggregatorResponse = await httpClient.MakeBankOfGeorgiaRequest<SubmitOrderAggregatorResponse>(requestMessage, serializer);
        SubmitOrderResponse response = aggregatorResponse.ToSubmitOrderResponse();

        if (string.IsNullOrWhiteSpace(response.Id))
        {
            throw new BankOfGeorgiaApiException($"{nameof(SubmitOrder)} resulted in an empty {nameof(response.Id)}", aggregatorResponse);
        }

        return response;
    }

    public async Task<GetOrderDetailsResponse> GetOrderDetails(GetOrderDetailsRequest request)
    {
        string url = $"v1/receipt/{request.OrderId}";
        HttpRequestMessage requestMessage = await CreateAuthenticatedRequestMessage(HttpMethod.Get, url);

        var aggregatorResponse = await httpClient.MakeBankOfGeorgiaRequest<GetOrderDetailsAggregatorResponse>(requestMessage, serializer);
        GetOrderDetailsResponse response = aggregatorResponse.ToGetOrderDetailsResponse();

        if (string.IsNullOrWhiteSpace(response.OrderId))
        {
            throw new BankOfGeorgiaApiException($"{nameof(GetOrderDetails)} resulted in an empty {nameof(response.OrderId)}", aggregatorResponse);
        }

        return response;
    }

    public async Task<SaveCardForRecurringPaymentsResponse> SaveCardForRecurringPayments(SaveCardForRecurringPaymentsRequest request)
    {
        string url = $"v1/orders/{request.OrderId}/cards";
        HttpRequestMessage requestMessage = await CreateAuthenticatedRequestMessage(HttpMethod.Put, url);

        if (request.IdempotencyKey is not null)
        {
            requestMessage.Headers.Add("Idempotency-Key", serializer.Serialize(request.IdempotencyKey));
        }

        await httpClient.MakeBankOfGeorgiaRequestExpectingAccepted(requestMessage, nameof(SaveCardForRecurringPayments));
        return new SaveCardForRecurringPaymentsResponse { Success = true };
    }

    public async Task<SaveCardForAutomaticPaymentsResponse> SaveCardForAutomaticPayments(SaveCardForAutomaticPaymentsRequest request)
    {
        string url = $"v1/orders/{request.OrderId}/subscriptions";
        HttpRequestMessage requestMessage = await CreateAuthenticatedRequestMessage(HttpMethod.Put, url);

        if (request.IdempotencyKey is not null)
        {
            requestMessage.Headers.Add("Idempotency-Key", serializer.Serialize(request.IdempotencyKey));
        }

        await httpClient.MakeBankOfGeorgiaRequestExpectingAccepted(requestMessage, nameof(SaveCardForAutomaticPayments));
        return new SaveCardForAutomaticPaymentsResponse { Success = true };
    }

    public async Task<DeleteSavedCardResponse> DeleteSavedCard(DeleteSavedCardRequest request)
    {
        string url = $"v1/charges/card/{request.OrderId}";
        HttpRequestMessage requestMessage = await CreateAuthenticatedRequestMessage(HttpMethod.Delete, url);

        if (request.IdempotencyKey is not null)
        {
            requestMessage.Headers.Add("Idempotency-Key", serializer.Serialize(request.IdempotencyKey));
        }

        await httpClient.MakeBankOfGeorgiaRequestExpectingAccepted(requestMessage, nameof(DeleteSavedCard));
        return new DeleteSavedCardResponse { Success = true };
    }

    private async Task<HttpRequestMessage> CreateAuthenticatedRequestMessage(HttpMethod method, string url)
    {
        TokenApiResponse token = await bankOfGeorgiaApiTokenClient.GetToken();
        HttpRequestMessage requestMessage = new(method, url);
        AuthenticationHeaderValue authHeader = new("Bearer", token.AccessToken);
        requestMessage.Headers.Authorization = authHeader;
        return requestMessage;
    }
}
