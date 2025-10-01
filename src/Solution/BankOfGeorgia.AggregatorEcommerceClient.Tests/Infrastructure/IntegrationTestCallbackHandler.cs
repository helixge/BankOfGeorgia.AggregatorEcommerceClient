namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class IntegrationTestCallbackHandler : IBankOfGeorgiaAggregatorCallbackHandler
{
    public Task Handle(CallbackRequest request)
    {
        return Task.CompletedTask;
    }
}
