using BankOfGeorgia.AggregatorEcommerceClient;

namespace EcommerceWebAppExample;

public class BogAggregatorCallbackHandler(
    ILogger<BogAggregatorCallbackHandler> logger
    ) : IBankOfGeorgiaAggregatorCallbackHandler
{
    public async Task Handle(CallbackRequest request)
    {
        await Task.Delay(0);
        logger.LogInformation("Recieved callback from BOG Aggregator Ecommenre API. {@Request}", request);
    }
}
