using System;
using System.ComponentModel.DataAnnotations;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaAggregatorEcommerceClientOptions
{
    [Required(ErrorMessage = "ClientId is required")]
    public required string ClientId { get; set; }

    [Required(ErrorMessage = "ClientSecret is required")]
    public required string ClientSecret { get; set; }

    [Url]
    public string? OAuthUrl { get; set; }

    [Url]
    public string? ApiBaseUrl { get; set; }

    public string OAuthUrlOrDefault()
    {
        if (string.IsNullOrWhiteSpace(OAuthUrl))
        {
            return Constants.DefaultOAuthUrl;
        }

        return OAuthUrl;
    }

    public string ApiBaseUrlOrDefault()
    {
        if (string.IsNullOrWhiteSpace(ApiBaseUrl))
        {
            return Constants.DefaultBaseUrl;
        }

        if (ApiBaseUrl.EndsWith("/") is false)
        {
            return $"{ApiBaseUrl}/";
        }
        return ApiBaseUrl;
    }
}
