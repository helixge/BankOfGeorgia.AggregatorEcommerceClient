using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class JsonPropertyNameEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private static readonly ConcurrentDictionary<Type, (Dictionary<string, TEnum> JsonToEnum, Dictionary<TEnum, string> EnumToJson)> Cache = new();

    private readonly Dictionary<string, TEnum> _jsonToEnum;
    private readonly Dictionary<TEnum, string> _enumToJson;
    private readonly bool _isNullable;
    private readonly Type _underlyingType;

    public JsonPropertyNameEnumConverter()
    {
        _underlyingType = typeof(TEnum);
        _isNullable = false;

        var mappings = Cache.GetOrAdd(_underlyingType, _ => BuildMappings());
        _jsonToEnum = mappings.JsonToEnum;
        _enumToJson = mappings.EnumToJson;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(TEnum) || typeToConvert == typeof(TEnum?);
    }

    private static (Dictionary<string, TEnum> JsonToEnum, Dictionary<TEnum, string> EnumToJson) BuildMappings()
    {
        Dictionary<string, TEnum> jsonToEnum = new();
        Dictionary<TEnum, string> enumToJson = new();

        foreach (TEnum value in Enum.GetValues<TEnum>())
        {
            FieldInfo field = typeof(TEnum).GetField(value.ToString()!)!;
            JsonPropertyNameAttribute? attribute = field.GetCustomAttribute<JsonPropertyNameAttribute>();

            if (attribute is not null)
            {
                jsonToEnum[attribute.Name] = value;
                enumToJson[value] = attribute.Name;
            }
            else
            {
                jsonToEnum[value.ToString()] = value;
                enumToJson[value] = value.ToString();
            }
        }

        return (jsonToEnum, enumToJson);
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            if (Nullable.GetUnderlyingType(typeToConvert) != null)
            {
                return default;
            }
            throw new JsonException($"Cannot convert null value to non-nullable {typeof(TEnum).Name}");
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string value for {typeof(TEnum).Name}");
        }

        string? value = reader.GetString();
        if (value is not null && _jsonToEnum.TryGetValue(value, out var enumValue))
        {
            return enumValue;
        }

        throw new JsonException($"Unknown {typeof(TEnum).Name} value: {value}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_enumToJson.TryGetValue(value, out var jsonValue))
        {
            writer.WriteStringValue(jsonValue);
        }
        else
        {
            throw new JsonException($"Unknown {typeof(TEnum).Name} value: {value}");
        }
    }
}

public class NullableJsonPropertyNameEnumConverter<TEnum> : JsonConverter<TEnum?> where TEnum : struct, Enum
{
    private readonly JsonPropertyNameEnumConverter<TEnum> _innerConverter;

    public NullableJsonPropertyNameEnumConverter()
    {
        _innerConverter = new JsonPropertyNameEnumConverter<TEnum>();
    }

    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return _innerConverter.Read(ref reader, typeof(TEnum), options);
    }

    public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        _innerConverter.Write(writer, value.Value, options);
    }
}