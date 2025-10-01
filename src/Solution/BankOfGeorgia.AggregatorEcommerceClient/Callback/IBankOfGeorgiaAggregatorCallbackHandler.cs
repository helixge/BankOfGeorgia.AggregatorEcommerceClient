using BankOfGeorgia.AggregatorEcommerceClient;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaAggregatorCallbackHandler
{
    Task Handle(CallbackRequest request);
}
