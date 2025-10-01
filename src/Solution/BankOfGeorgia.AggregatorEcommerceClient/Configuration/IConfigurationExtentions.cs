using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public static class IConfigurationExtentions
{
    internal const string DefaultConfigurationSectionKey = "BankOfGeorgiaAggregatorEcommerce";

    public static void AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
        this IHostApplicationBuilder host)
        where TCallbackHandler : class, IBankOfGeorgiaAggregatorCallbackHandler
        => AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
            host,
            DefaultConfigurationSectionKey);

    public static void AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
        this IHostApplicationBuilder host,
        string configurationSectionKey)
        where TCallbackHandler : class, IBankOfGeorgiaAggregatorCallbackHandler
        => host.Services.AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
            host.Configuration,
            configurationSectionKey);

    public static IServiceCollection AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TCallbackHandler : class, IBankOfGeorgiaAggregatorCallbackHandler
        => AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
            services,
            configuration,
            DefaultConfigurationSectionKey);

    public static IServiceCollection AddBankOfGeorgiaAggregatorEcommerce<TCallbackHandler>(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationSectionKey)
        where TCallbackHandler : class, IBankOfGeorgiaAggregatorCallbackHandler
    {
        services
            .AddScoped<IBankOfGeorgiaAggregatorCallbackHandler, TCallbackHandler>();

        services
            .AddOptions<BankOfGeorgiaAggregatorEcommerceClientOptions>()
            .Bind(configuration.GetSection(configurationSectionKey))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddSingleton<IBankOfGeorgiaApiSerializationService, BankOfGeorgiaApiSerializationService>()
            .AddSingleton<ICallbackRequestVerificationService, CallbackRequestVerificationService>();

        services
            .AddScoped<IBankOfGeorgiaAggregatorEcommerceClient, BankOfGeorgiaAggregatorEcommerceClient>()
            .AddScoped<IBankOfGeorgiaApiTokenClient, BankOfGeorgiaApiTokenClient>();

        services
            .AddHttpClient<IBankOfGeorgiaAggregatorEcommerceClient, BankOfGeorgiaAggregatorEcommerceClient>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BankOfGeorgiaAggregatorEcommerceClientOptions>>().Value;
                var url = options.ApiBaseUrlOrDefault();
                httpClient.BaseAddress = new Uri(url);
            });

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
