using BankOfGeorgia.AggregatorEcommerceClient;
using EcommerceWebAppExample;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.AddBankOfGeorgiaAggregatorEcommerce<BogAggregatorCallbackHandler>();
// Or if you want to use an extension for IServiceCollection, call:
// builder.Services.AddBankOfGeorgiaAggregatorEcommerce<BogAggregatorCallbackHandler>(builder.Configuration);

var app = builder.Build();

app.UseBankOfGeorgiaCallback("/bog/callback");

app.MapGet("/bog/return/success",
    (
        ILogger<Program> logger,
        [FromQuery] string? orderId = null
    ) =>
    {
        logger.LogInformation("Received success return for Order ID: {OrderId}", orderId);
    });

app.MapGet("/bog/return/fail",
    (
        ILogger<Program> logger,
        [FromQuery] string? orderId = null
    ) =>
    {
        logger.LogWarning("Received fail return for Order ID: {OrderId}", orderId);
    });

app.MapGet("/",
    (
        ILogger<Program> logger,
        [FromQuery] string? orderId = null
    ) =>
    {
        return Results.Content("""
            <form action="pay" method="post" enctype="multipart/form-data">
                Amount: <input type="number" name="amount" value="2.3" step="0.01">
                <br/>
                <button type="submit">Pay</button>
            </form>
            """, "text/html");
    });

app.MapPost("/pay",
    async (
        ILogger<Program> logger,
        IBankOfGeorgiaAggregatorEcommerceClient bog,
        [FromForm] decimal amount
    ) =>
    {
        string orderId = Guid.NewGuid().ToString();
        SubmitOrderRequest request = new()
        {
            ExternalOrderId = orderId,
            CallbackUrl = "https://mywebsite.ge/bog/callback",
            RedirectUrls = new RedirectUrls()
            {
                Success = $"http://localhost:4444/bog/return/success?id={orderId}",
                Fail = $"http://localhost:4444/bog/return/fail?id={orderId}"
            },
            PaymentMethod = [PaymentMethod.Card],
            PurchaseUnits = new PurchaseUnits()
            {
                TotalAmount = amount,
                Basket =
                [
                    new BasketItem()
                    {
                        ProductId = "Example product",
                        Quantity = 1,
                        UnitPrice = amount,
                    },
                ]
            },

            // Other optional propeties
        };
        SubmitOrderResponse response = await bog.SubmitOrder(request);
        return Results.Redirect(response.RedirectLink, permanent: false);
    }).DisableAntiforgery();

await app.RunAsync();
