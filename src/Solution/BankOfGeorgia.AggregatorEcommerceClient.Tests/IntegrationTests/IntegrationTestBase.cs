using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class IntegrationTestBase : IDisposable
{
    public const string EcommerceWebAppExampleCallbackUrl = "https://localhost:4443/api/bog/callback";
    public const string EcommerceWebAppExampleSuccessUrl = "https://localhost:4443/return/bog/success?orderid=43";
    public const string EcommerceWebAppExampleFailUrl = "https://localhost:4443/return/bog/fail?orderid=175";

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
        builder.Configuration.AddUserSecrets<IntegrationTestBase>();
        builder.AddBankOfGeorgiaAggregatorEcommerce<IntegrationTestCallbackHandler>();

        IHost host = builder.Build();
        return host;
    }
}
