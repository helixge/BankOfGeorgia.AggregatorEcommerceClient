using Microsoft.Extensions.Hosting;
    
namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class IntegrationTestBase : IDisposable
{
    protected IHost App { get; init; }

    public IntegrationTestBase()
    {
        App = BuildHost();
    }

    public void Dispose()
    {
        App.Dispose();
    }

    private IHost BuildHost()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Services.AddBankOfGeorgiaAggregatorEcommerce(
            builder.Configuration
            );

        IHost host = builder.Build();
        return host;
    }
}
