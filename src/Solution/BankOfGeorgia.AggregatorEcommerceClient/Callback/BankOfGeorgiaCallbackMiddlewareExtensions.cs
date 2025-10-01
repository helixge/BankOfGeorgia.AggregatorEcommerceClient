using Microsoft.AspNetCore.Builder;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public static class BankOfGeorgiaCallbackMiddlewareExtensions
{
    public static IApplicationBuilder UseBankOfGeorgiaCallback(
        this IApplicationBuilder builder,
        string callbackPath)
    {
        return builder.UseMiddleware<BankOfGeorgiaCallbackMiddleware>(callbackPath);
    }
}
