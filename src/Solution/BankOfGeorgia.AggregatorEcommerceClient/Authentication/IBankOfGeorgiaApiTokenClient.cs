namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaApiTokenClient
{
    Task<TokenApiResponse> GetToken();
}
