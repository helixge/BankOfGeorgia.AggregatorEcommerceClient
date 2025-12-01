using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class SubmitSubscriptionPaymentOrderAggregatorResponse
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("_links")]
    public AutomaticPaymentOrderLinks Links { get; set; } = new();

    internal SubmitSubscriptionPaymentOrderResponse ToSubmitSubscriptionPaymentOrderResponse()
       => new()
       {
           Id = Id,
           DetailLink = Links?.Details?.Href
       };
}
