namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaApiSerializationService
{
    TData? Deserialize<TData>(string serialized);

    string Serialize<TData>(TData obj);
}
