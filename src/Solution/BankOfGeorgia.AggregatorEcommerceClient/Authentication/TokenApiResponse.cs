using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class TokenApiResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    [JsonPropertyName("access_token")]
    public string? TokenType { get; init; }

    [JsonPropertyName("access_token")]
    public int ExpiresIn { get; init; }
}
