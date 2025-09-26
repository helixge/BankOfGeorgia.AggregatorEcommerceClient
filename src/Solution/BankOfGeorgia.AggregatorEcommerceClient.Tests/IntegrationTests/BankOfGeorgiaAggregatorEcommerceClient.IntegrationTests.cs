using Microsoft.Extensions.DependencyInjection;

namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class BankOfGeorgiaAggregatorEcommerceClientTests : IntegrationTestBase
{
    [Fact]
    public async Task SubmitOrder_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest request = new()
        {
            CallbackUrl = "https://localhost",
            PurchaseUnits = new PurchaseUnits()
            {
                TotalAmount = 17.30m,
                Basket =
                [
                    new BasketItem()
                    {
                        ProductId = "1",
                        Quantity = 1,
                        UnitPrice = 13.22m,
                    },
                    new BasketItem()
                    {
                        ProductId = "1",
                        Quantity = 2,
                        UnitPrice = 1.04m,
                    }
                ],
                Delivery = new Delivery()
                {
                    Amount = 2.00m,
                }
            },
            PaymentMethod =
            [
                 PaymentMethod.Card,
                 PaymentMethod.GooglePay,
                 PaymentMethod.ApplePay,
            ]
        };

        // Act
        SubmitOrderResponse response = await client.SubmitOrder(request);

        // Assert
        Assert.Multiple(
            () => Assert.True(!string.IsNullOrWhiteSpace(response.Id)),
            () => Assert.True(!string.IsNullOrWhiteSpace(response.DetailLink)),
            () => Assert.True(!string.IsNullOrWhiteSpace(response.RedirectLink))
        );
    }
}
