using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

internal class BankOfGeorgiaApiSerializationService : IBankOfGeorgiaApiSerializationService
{
    private readonly JsonSerializerOptions _serializerOptions;

    public BankOfGeorgiaApiSerializationService()
    {
        _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new EnumConverterFactory() }
        };
    }

    public string Serialize<TData>(TData data)
    {
        return JsonSerializer.Serialize(data, _serializerOptions);
    }

    public TData? Deserialize<TData>(string serialized)
    {
        try
        {
            return JsonSerializer.Deserialize<TData>(serialized, _serializerOptions);
        }
        catch (Exception ex)
        {
            throw new BankOfGeorgiaApiException($"Failed to deserialize the following text to type '{typeof(TData).Name}': {serialized}", ex);
        }
    }
}
