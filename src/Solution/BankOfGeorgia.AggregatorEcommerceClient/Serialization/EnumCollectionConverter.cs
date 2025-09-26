using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class EnumCollectionConverter<TEnum> : JsonConverter<IEnumerable<TEnum>> where TEnum : struct, Enum
{
    private static readonly ConcurrentDictionary<Type, (Dictionary<string, TEnum> JsonToEnum, Dictionary<TEnum, string> EnumToJson)> Cache = new();

    private readonly Dictionary<string, TEnum> _jsonToEnum;
    private readonly Dictionary<TEnum, string> _enumToJson;

    public EnumCollectionConverter()
    {
        var mappings = Cache.GetOrAdd(typeof(TEnum), _ => BuildMappings());
        _jsonToEnum = mappings.JsonToEnum;
        _enumToJson = mappings.EnumToJson;
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

    public override IEnumerable<TEnum>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected array");
        }

        List<TEnum> list = new();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (value is not null &&
                    _jsonToEnum.TryGetValue(value, out var enumValue))
                {
                    list.Add(enumValue);
                }
                else
                {
                    throw new JsonException($"Unknown {typeof(TEnum).Name} value: {value}");
                }
            }
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<TEnum> value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (TEnum enumValue in value)
        {
            if (_enumToJson.TryGetValue(enumValue, out var jsonValue))
            {
                writer.WriteStringValue(jsonValue);
            }
            else
            {
                throw new JsonException($"Unknown {typeof(TEnum).Name} value: {enumValue}");
            }
        }

        writer.WriteEndArray();
    }
}
