# Bank of Georgia Aggregator Integration Protocol .NET Client for E-Commerce Services

[![NuGet Version](https://img.shields.io/nuget/v/Helix.BankOfGeorgia.AggregatorEcommerceClient)](https://www.nuget.org/packages/Helix.BankOfGeorgia.AggregatorEcommerceClient)

[Helix.BankOfGeorgia.AggregatorEcommerceClient](https://www.nuget.org/packages/Helix.BankOfGeorgia.AggregatorEcommerceClient) is a .NET client library for using the Bank of Georgia e-commerce payments gateway.

## How To Use
See [ASP.NET Core integration guide](#integrating-with-aspnet-core) below

### Define Options

Configure the client with your Bank of Georgia credentials:

```csharp
var clientOptions = new BankOfGeorgiaAggregatorEcommerceClientOptions()
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret"
};
```

* **ClientId** (string) - **Required**
  Your client ID provided by Bank of Georgia

* **ClientSecret** (string) - **Required**
  Your client secret provided by the Bank of Georgia

## Payment and Transaction Related Methods

### Core Payment Operations

* **SubmitOrder**
  Submit a new payment order to Bank of Georgia for processing. Returns order details, including a redirect URL where the customer should be directed to complete the payment.
  ```csharp
  var response = await client.SubmitOrder(new SubmitOrderRequest
  {
      ExternalOrderId = "your-order-id",
      PurchaseUnits = purchaseUnits,
      RedirectUrls = redirectUrls,
      // Optional parameters
      IdempotencyKey = "unique-key",
      Language = UiLanguage.EN,
      Theme = UiTheme.Light
  });
  // Redirect customer to: response.Links.Redirect.Href
  ```

* **GetOrderDetails**
  Retrieve detailed information about a previously submitted order, including its current status and payment details.
  ```csharp
  var details = await client.GetOrderDetails(new GetOrderDetailsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **RefundOrder**
  Process a full or partial refund for a completed order. The refund process may take several bank days to complete.
  ```csharp
  var refund = await client.RefundOrder(new RefundOrderRequest
  {
      OrderId = "bank-order-id",
      Amount = 100.50m,
  });
  ```

### Card Management Operations

* **SaveCardForRecurringPayments**
  Mark an order to save the customer's card information for future recurring payments. This must be enabled by Bank of Georgia for your merchant account.
  ```csharp
  var result = await client.SaveCardForRecurringPayments(new SaveCardForRecurringPaymentsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **SaveCardForAutomaticPayments**
  Mark an order to save the customer's card for automatic/subscription payments. This must be enabled by the Bank of Georgia for your merchant account.
  ```csharp
  var result = await client.SaveCardForAutomaticPayments(new SaveCardForAutomaticPaymentsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **DeleteSavedCard**
  Remove a previously saved card from the system.
  ```csharp
  var result = await client.DeleteSavedCard(new DeleteSavedCardRequest
  {
      OrderId = "bank-order-id"
  });
  ```

## Callback Handling

Bank of Georgia sends callback notifications about order status changes. The library provides middleware and handler interfaces to process these callbacks securely with signature verification.

### Implementing a Callback Handler

Create a class that implements `IBankOfGeorgiaAggregatorCallbackHandler`:

```csharp
public class YourCallbackHandler : IBankOfGeorgiaAggregatorCallbackHandler
{
    private readonly ILogger<YourCallbackHandler> _logger;

    public YourCallbackHandler(ILogger<YourCallbackHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(CallbackRequest request)
    {
        _logger.LogInformation("Order {OrderId} status changed to {Status}",
            request.OrderId, request.Status);

        await Task.CompletedTask;
    }
}
```

## Integrating with ASP.NET Core

Follow these steps to integrate the client with your ASP.NET Core application:

### 1. Configure appsettings.json

Add the Bank of Georgia configuration section to your `appsettings.json`:

```json
{
  "BankOfGeorgiaAggregatorEcommerce": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

### 2. Register Services in Program.cs

Register the Bank of Georgia services and your callback handler:

```csharp
using BankOfGeorgia.AggregatorEcommerceClient;

var builder = WebApplication.CreateBuilder(args);

// Option 1: Register Bank of Georgia services with your callback handler
builder.Services
    .AddBankOfGeorgiaAggregatorEcommerce<YourCallbackHandler>(
    builder.Configuration);

// Option2: Use a custom configuration section name
builder.Services
    .AddBankOfGeorgiaAggregatorEcommerce<YourCallbackHandler>(
    builder.Configuration,
    "CustomSectionName");

// Option 3: Use the IHostApplicationBuilder extension (recommended)
builder.AddBankOfGeorgiaAggregatorEcommerce<YourCallbackHandler>();

var app = builder.Build();
```

### 3. Configure Middleware

Add the callback middleware to handle Bank of Georgia webhook notifications:

```csharp
var app = builder.Build();

// Configure the callback endpoint
app.UseBankOfGeorgiaCallback("/api/bog/callback");

// Configure your other middleware and routes...

await app.RunAsync();
```

The callback middleware automatically:
- Validates request signatures for security
- Deserializes the callback payload
- Invokes your registered callback handler
- Returns appropriate HTTP status codes

### 4. Inject and Use the Client

Inject `IBankOfGeorgiaAggregatorEcommerceClient` into your controllers or services:

```csharp
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IBankOfGeorgiaAggregatorEcommerceClient _bogClient;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IBankOfGeorgiaAggregatorEcommerceClient bogClient,
        ILogger<PaymentController> logger)
    {
        _bogClient = bogClient;
        _logger = logger;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var request = new SubmitOrderRequest
        {
            ExternalOrderId = dto.OrderId,
            PurchaseUnits = new PurchaseUnits
            {
                Currency = "GEL",
                TotalAmount = dto.Amount,
                BasketItems = dto.Items.Select(i => new BasketItem
                {
                    ProductId = i.ProductId,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.Price,
                    TotalPrice = i.Quantity * i.Price
                }).ToList()
            },
            RedirectUrls = new RedirectUrls
            {
                Fail = "https://yoursite.com/payment/failed",
                Success = "https://yoursite.com/payment/success"
            }
        };

        var response = await _bogClient.SubmitOrder(request);

        // Return the redirect URL to the frontend
        return Ok(new { RedirectUrl = response.Links.Redirect.Href });
    }

    [HttpGet("order-status/{orderId}")]
    public async Task<IActionResult> GetOrderStatus(string orderId)
    {
        var details = await _bogClient.GetOrderDetails(new GetOrderDetailsRequest
        {
            OrderId = orderId
        });

        return Ok(details);
    }
}
```

## Complete Example Application

See the `EcommerceWebAppExample` project in the repository for a complete working example demonstrating:
- Service registration and configuration
- Callback handler implementation
- Middleware setup
- Success/failure redirect handling

## Error Handling

The library throws `BankOfGeorgiaApiException` for API-related errors and `CallbackValidationException` for callback signature validation failures. Always wrap API calls in try-catch blocks:

```csharp
try
{
    var response = await _bogClient.SubmitOrder(request);
    // Process successful response
}
catch (BankOfGeorgiaApiException ex)
{
    _logger.LogError(ex, "Bank of Georgia API error");
    // Handle API error
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    // Handle unexpected error
}
```

## Requirements

- .NET 8.0 or higher
- Active Bank of Georgia merchant account with API credentials
