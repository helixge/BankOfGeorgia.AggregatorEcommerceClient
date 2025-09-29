
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

        SubmitOrderRequest request = CreateValidSubmitOrderRequest();

        // Act
        SubmitOrderResponse response = await client.SubmitOrder(request);

        // Assert
        Assert.Multiple(
            () => Assert.True(!string.IsNullOrWhiteSpace(response.Id)),
            () => Assert.True(!string.IsNullOrWhiteSpace(response.DetailLink)),
            () => Assert.True(!string.IsNullOrWhiteSpace(response.RedirectLink))
        );
    }

    [Fact]
    public async Task GetOrderDetails_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest orderRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse orderResponse = await client.SubmitOrder(orderRequest);

        GetOrderDetailsRequest request = new()
        {
            OrderId = orderResponse.Id!
        };

        // Act
        GetOrderDetailsResponse response = await client.GetOrderDetails(request);

        // Assert
        Assert.Multiple(
            () => Assert.True(Guid.TryParse(response.OrderId, out _)),
            () => Assert.True(Guid.TryParse(response.ExternalOrderId, out _)),
            () => Assert.False(response.OrderId == response.ExternalOrderId),
            () => Assert.True(response.Capture == CaptureType.Automatic),
            () => Assert.Equal(CaptureType.Automatic, response.Capture),
            () => Assert.NotNull(response.OrderStatus),
            () => Assert.Equal(OrderStatusType.Created, response.OrderStatus!.Key),
            () => Assert.NotNull(response.PaymentDetail),
            () => Assert.NotNull(response.PaymentDetail!.TransferMethod),
            () => Assert.Equal(PaymentMethod.Unknown, response.PaymentDetail!.TransferMethod!.Key)
        );
    }

    [Fact]
    public async Task SaveCardForRecurringPayments_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest orderRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse orderResponse = await client.SubmitOrder(orderRequest);

        SaveCardForRecurringPaymentsRequest request = new()
        {
            OrderId = orderResponse.Id!,
            IdempotencyKey = Guid.NewGuid()
        };

        // Act
        SaveCardForRecurringPaymentsResponse response = await client.SaveCardForRecurringPayments(request);

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task SaveCardForAutomaticPayments_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest orderRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse orderResponse = await client.SubmitOrder(orderRequest);

        SaveCardForAutomaticPaymentsRequest request = new()
        {
            OrderId = orderResponse.Id!,
            IdempotencyKey = Guid.NewGuid()
        };

        // Act
        SaveCardForAutomaticPaymentsResponse response = await client.SaveCardForAutomaticPayments(request);

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task DeleteSavedCard_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest orderRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse orderResponse = await client.SubmitOrder(orderRequest);

        DeleteSavedCardRequest request = new()
        {
            OrderId = orderResponse.Id!,
            IdempotencyKey = Guid.NewGuid()
        };

        // Act
        DeleteSavedCardResponse response = await client.DeleteSavedCard(request);

        // Assert
        Assert.True(response.Success);
    }

    private SubmitOrderRequest CreateValidSubmitOrderRequest()
    {
        SubmitOrderRequest request = new()
        {
            CallbackUrl = "https://localhost/callback",
            RedirectUrls = new RedirectUrls()
            {
                Success = "https://localhost/success",
                Fail = "https://localhost/fail"
            },
            PurchaseUnits = new PurchaseUnits()
            {
                TotalAmount = 5.30m,
                Basket =
                [
                    new BasketItem()
                    {
                        ProductId = "1",
                        Quantity = 1,
                        UnitPrice = 1.22m,
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
                 PaymentMethod.Card
            ],
            ExternalOrderId = Guid.NewGuid().ToString()
        };

        return request;
    }
}
