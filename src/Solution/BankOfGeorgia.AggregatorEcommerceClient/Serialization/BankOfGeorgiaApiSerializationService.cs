using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public interface IBankOfGeorgiaApiSerializationService
{
    TData? Deserialize<TData>(string data);

    string Serialize<TData>(TData data);
}

internal class BankOfGeorgiaApiSerializationService : IBankOfGeorgiaApiSerializationService
{
    private readonly JsonSerializerOptions _serializerOptions;

    public BankOfGeorgiaApiSerializationService()
    {
        _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public string Serialize<TData>(TData data)
    {
        return JsonSerializer.Serialize(data, _serializerOptions);
    }

    public TData? Deserialize<TData>(string data)
    {
        return JsonSerializer.Deserialize<TData>(data, _serializerOptions);
    }
}
