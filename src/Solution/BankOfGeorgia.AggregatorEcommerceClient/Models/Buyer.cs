using System.Text.Json.Serialization;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class Buyer
{
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }

    [JsonPropertyName("masked_email")]
    public string? MaskedEmail { get; set; }

    [JsonPropertyName("masked_phone")]
    public string? MaskedPhone { get; set; }
}
