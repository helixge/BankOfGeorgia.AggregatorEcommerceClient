using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitOrderAggregatorResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("_links")]
    public PaymentOrderLinks Links { get; set; } = new();

    internal SubmitOrderResponse ToSubmitOrderResponse()
        => new()
        {
            Id = Id,
            DetailLink = Links?.Details?.Href,
            RedirectLink = Links?.Redirect?.Href
        };
}
