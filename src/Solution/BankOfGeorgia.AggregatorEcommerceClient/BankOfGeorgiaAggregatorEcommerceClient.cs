using Microsoft.Extensions.Options;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaAggregatorEcommerceClient
{
    Task SubmitOrder(SubmitOrderRequest submitOrderRequest);
}


internal class BankOfGeorgiaAggregatorEcommerceClient(
    IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions> options,
    IBankOfGeorgiaApiTokenClient bankOfGeorgiaApiTokenClient,
    IBankOfGeorgiaApiSerializationService serializer,
    HttpClient httpClient
) : IBankOfGeorgiaAggregatorEcommerceClient
{
    public async Task SubmitOrder(SubmitOrderRequest submitOrderRequest)
    {
        TokenApiResponse token = await bankOfGeorgiaApiTokenClient.GetToken();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "v1/ecommerce/orders");
        string serializedContent = serializer.Serialize(submitOrderRequest);
        request.Content = new StringContent(serializedContent);

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        throw new NotImplementedException();
    }
}
