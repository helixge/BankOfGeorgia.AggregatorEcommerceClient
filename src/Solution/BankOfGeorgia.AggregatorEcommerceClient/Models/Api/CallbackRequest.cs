using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class CallbackRequest
{
    [JsonPropertyName("event")]
    public string? Event { get; init; }

    [JsonPropertyName("zoned_request_time")]
    public string? ZonedRequestTime { get; init; }

    [JsonPropertyName("body")]
    public GetOrderDetailsAggregatorResponse? Body { get; set; }
}
