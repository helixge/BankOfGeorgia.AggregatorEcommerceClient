namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaApiSerializationService
{
    string Serialize<TData>(TData obj);
    
    TData? Deserialize<TData>(string serialized);

    TData? Deserialize<TData>(Span<byte> serialized);
}
