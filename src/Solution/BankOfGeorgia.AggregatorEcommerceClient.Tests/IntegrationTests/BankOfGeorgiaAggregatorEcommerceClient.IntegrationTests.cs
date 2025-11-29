
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
    public async Task SubmitOrderWithParentOrderId_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest request = CreateValidSubmitOrderRequest();

        // Act
        SubmitOrderResponse response = await client.SubmitOrder(request, "6dc9dba8-bdcd-4dbe-8f59-1b429f1a1707");

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
            OrderId = "efa0fa1b-6946-4c2b-852f-417cbea53ad1"
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
            OrderId = orderResponse.Id!
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
            OrderId = orderResponse.Id!
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

    [Fact]
    public async Task RefundOrder_ValidRequest_Succeeds()
    {
        // Arrange
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest orderRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse orderResponse = await client.SubmitOrder(orderRequest);

        RefundOrderRequest request = new()
        {
            OrderId = orderResponse.Id!,
            Amount = 2.50m,
            IdempotencyKey = Guid.NewGuid()
        };

        // Act
        RefundOrderResponse response = await client.RefundOrder(request);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(response.Key),
            () => Assert.NotNull(response.Message),
            () => Assert.NotNull(response.ActionId)
        );
    }

    private SubmitOrderRequest CreateValidSubmitOrderRequest()
    {
        SubmitOrderRequest request = new()
        {
            CallbackUrl = EcommerceWebAppExampleCallbackUrl,
            RedirectUrls = new RedirectUrls()
            {
                Success = EcommerceWebAppExampleSuccessUrl,
                Fail = EcommerceWebAppExampleFailUrl
            },
            PurchaseUnits = new PurchaseUnits()
            {
                TotalAmount = 1.28m,
                Basket =
                [
                    new BasketItem()
                    {
                        ProductId = "1",
                        Quantity = 1,
                        UnitPrice = 0.20m,
                    },
                    new BasketItem()
                    {
                        ProductId = "1",
                        Quantity = 2,
                        UnitPrice = 0.04m,
                    }
                ],
                Delivery = new Delivery()
                {
                    Amount = 1.00m,
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
