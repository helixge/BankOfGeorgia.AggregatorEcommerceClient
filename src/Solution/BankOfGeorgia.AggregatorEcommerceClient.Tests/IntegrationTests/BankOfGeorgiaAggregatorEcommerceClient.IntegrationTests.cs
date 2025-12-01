
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

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

    [Fact]
    public async Task TestRecurringFlow()
    {
        using IServiceScope scope = App.Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IBankOfGeorgiaAggregatorEcommerceClient>();

        SubmitOrderRequest order1SubmitRequest = CreateValidSubmitOrderRequest();
        SubmitOrderResponse order1SubmitResponse = await client.SubmitOrder(order1SubmitRequest);

        GetOrderDetailsRequest order1DetailsRequest = new()
        {
            OrderId = order1SubmitResponse.Id!
        };
        GetOrderDetailsResponse order1DetailsResponse = await client.GetOrderDetails(order1DetailsRequest);


        SaveCardForRecurringPaymentsRequest saveCardRequest = new()
        {
            OrderId = order1SubmitResponse.Id!
        };
        SaveCardForRecurringPaymentsResponse saveCardResponse = await client.SaveCardForRecurringPayments(saveCardRequest);

        Process.Start(new ProcessStartInfo(order1SubmitResponse.RedirectLink!)
        {
            UseShellExecute = true
        });

        while (true)
        {
            await Task.Delay(2000);

            GetOrderDetailsRequest order1LoopDetailsRequest = new()
            {
                OrderId = order1SubmitResponse.Id!
            };
            GetOrderDetailsResponse order1LoopDetailsResponse = await client.GetOrderDetails(order1LoopDetailsRequest);

            if (order1LoopDetailsResponse.OrderStatus!.Key == OrderStatusType.Completed)
            {
                break;
            }
        }

        SubmitOrderRequest order2SubmitRequest = CreateValidSubmitOrderRequest(deliveryAmount: 2.00m);
        SubmitOrderResponse order2SubmitResponse = await client.SubmitOrder(order2SubmitRequest, order1SubmitResponse.Id!);

        GetOrderDetailsRequest order2DetailsRequest = new()
        {
            OrderId = order2SubmitResponse.Id!
        };
        GetOrderDetailsResponse order2DetailsResponse = await client.GetOrderDetails(order2DetailsRequest);
    }

    private SubmitOrderRequest CreateValidSubmitOrderRequest(decimal deliveryAmount = 1.00m)
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
                    Amount = deliveryAmount,
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
