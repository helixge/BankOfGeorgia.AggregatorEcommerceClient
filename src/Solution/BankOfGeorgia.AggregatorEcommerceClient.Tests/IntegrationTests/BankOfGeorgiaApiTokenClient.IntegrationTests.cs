using Microsoft.Extensions.DependencyInjection;

namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class BankOfGeorgiaApiTokenClientTests : IntegrationTestBase
{
    [Fact]
    public async Task GetToken_ValidCredentials_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaApiTokenClient>();

        // Act
        TokenApiResponse token = await client.GetToken();

        // Assert
        Assert.Multiple(
            () => Assert.StartsWith("eyJ", token.AccessToken),
            () => Assert.True(token.ExpiresIn > 0),
            () => Assert.Equal("Bearer", token.TokenType)
        );
    }
}
