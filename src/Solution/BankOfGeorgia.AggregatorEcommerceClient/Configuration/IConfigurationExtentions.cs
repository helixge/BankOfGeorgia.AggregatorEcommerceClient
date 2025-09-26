using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public static class IConfigurationExtentions
{
    public static IServiceCollection AddBankOfGeorgiaAggregatorEcommerce(
        this IServiceCollection services,
        IConfiguration configuration)
        => AddBankOfGeorgiaAggregatorEcommerce(services, configuration, "BankOfGeorgiaAggregatorEcommerce");

    public static IServiceCollection AddBankOfGeorgiaAggregatorEcommerce(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionKey)
    {
        services
            .AddOptions<BankOfGeorgiaAggregatorEcommerceClientOptions>()
            .Bind(configuration.GetSection(sectionKey))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddScoped<IBankOfGeorgiaAggregatorEcommerceClient, BankOfGeorgiaAggregatorEcommerceClient>()
            .AddScoped<IBankOfGeorgiaApiTokenClient, BankOfGeorgiaApiTokenClient>();

        services
            .AddHttpClient<IBankOfGeorgiaAggregatorEcommerceClient, BankOfGeorgiaAggregatorEcommerceClient>();

        services
            .AddHttpClient<IBankOfGeorgiaApiTokenClient, BankOfGeorgiaApiTokenClient>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions>>().Value;
                var basicAuthBytes = Encoding.UTF8.GetBytes($"{options.ClientId}:{options.ClientSecret}");
                var basicAuthValue = Convert.ToBase64String(basicAuthBytes);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthValue);
            });

        return services;
    }
}
