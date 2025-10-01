using BankOfGeorgia.AggregatorEcommerceClient;
using EcommerceWebAppExample;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//builder.AddBankOfGeorgiaAggregatorEcommerce<BogAggregatorCallbackHandler>();
builder.Services
    .AddBankOfGeorgiaAggregatorEcommerce<BogAggregatorCallbackHandler>(builder.Configuration);

var app = builder.Build();

app.UseBankOfGeorgiaCallback("/api/bog/callback");

app.MapGet(
    "/return/bog/success",
    (
        ILogger<Program> logger,
        [FromQuery] string? orderId = null
    ) =>
    {
        logger.LogInformation("Received success return for Order ID: {OrderId}", orderId);
    });

app.MapGet(
    "/return/bog/fail",
    (
        ILogger<Program> logger,
        [FromQuery] string? orderId = null
    ) =>
    {
        logger.LogWarning("Received fail return for Order ID: {OrderId}", orderId);
    });

await app.RunAsync();

