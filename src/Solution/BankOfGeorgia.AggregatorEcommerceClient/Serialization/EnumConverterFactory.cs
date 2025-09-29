using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class EnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum || (Nullable.GetUnderlyingType(typeToConvert)?.IsEnum ?? false);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(typeToConvert);

        if (underlyingType?.IsEnum == true)
        {
            Type converterType = typeof(NullableJsonPropertyNameEnumConverter<>).MakeGenericType(underlyingType);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }
        else if (typeToConvert.IsEnum)
        {
            Type converterType = typeof(JsonPropertyNameEnumConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }

        return null;
    }
}
