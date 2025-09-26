using Microsoft.Extensions.Options;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaAggregatorEcommerceClient
{

}


internal class BankOfGeorgiaAggregatorEcommerceClient(
    IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions> options,
    IBankOfGeorgiaApiTokenClient bankOfGeorgiaApiTokenClient,
    HttpClient httpClient
) : IBankOfGeorgiaAggregatorEcommerceClient
{
    public async Task SubmitOrder()
    {
        TokenApiResponse token = await bankOfGeorgiaApiTokenClient.GetToken();
        throw new NotImplementedException();
    }
}
