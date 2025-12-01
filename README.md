# Bank of Georgia Aggregator Integration Protocol .NET Client for E-Commerce Services

[![NuGet Version](https://img.shields.io/nuget/v/Helix.BankOfGeorgia.AggregatorEcommerceClient)](https://www.nuget.org/packages/Helix.BankOfGeorgia.AggregatorEcommerceClient)

[Helix.BankOfGeorgia.AggregatorEcommerceClient](https://www.nuget.org/packages/Helix.BankOfGeorgia.AggregatorEcommerceClient) is a .NET client library for using the Bank of Georgia e-commerce payments gateway.

## How To Use

> See [ASP.NET Core integration guide](#integrating-with-aspnet-core) below ⬇️

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

* **SubmitOrder** \
  Submit a new payment order to Bank of Georgia for processing. Returns order details, including a redirect URL where the customer should be directed to complete the payment.

  ```csharp
  var response = await client.SubmitOrder(new SubmitOrderRequest
  {
    ExternalOrderId = "254",
    CallbackUrl = "https://mywebsite.ge/payments/bog/callback",
    RedirectUrls = new RedirectUrls()
    {
        Success = "https://mywebsite.ge/payments/bog/success?id=254",
        Fail = "https://mywebsite.ge/payments/bog/fail?id=254"
    },
    PaymentMethod = [PaymentMethod.Card],
    PurchaseUnits = new PurchaseUnits()
    {
        TotalAmount = 12.60m,
        Basket =
        [
            new BasketItem()
            {
                ProductId = "1",
                Quantity = 1,
                UnitPrice = 5.00m,
            },
        ],
        Delivery = new Delivery()
        {
            Amount = 2.60m,
        }
    },
    
    // Other options propeties
  });
  ```
  Response contains a `Links.Redirect.Href` property containing a URL where the user must be redirected to complete the payment.

* **GetOrderDetails** \
  Retrieve detailed information about a previously submitted order, including its current status and payment details.
  ```csharp
  var details = await client.GetOrderDetails(new GetOrderDetailsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **RefundOrder** \
  Process a full or partial refund for a completed order. The refund process may take several bank days to complete.
  ```csharp
  var refund = await client.RefundOrder(new RefundOrderRequest
  {
      OrderId = "bank-order-id",
      Amount = 100.50m,
  });
  ```

### Recurring Payments

Recurring payments are not enabld by default but you can request this functionality from the Bank by contacting the directly.

The bank provides two ways to manage recurring payments:
1. **Recurring Payments** \
   The bank saves the card information based on a specific `order_id` and allows you to use that order as a parent order for future payments. You are allowed to change the amount for future payments. Customers may need to manually intervene for 3DS authorization where they must be redirected after the `SubmitOrder` operation execution.
       
2. **Automatic Subscription Payments**
   The bank saves the card information based on a specific `order_id` and allows you to use that order as a parent order for future payments. You are not allowed to change the amount for future payments and there is no user intervension required in the future.

### Card Management Operations
* **SaveCardForRecurringPayments** \
  You must call this method right after calling `SubmitOrder`, before you navigate the user to the payment page to tell the bank you want to save user's card for future payments.
  
  You will be able to use the supplied `bank-order-id` paramter as a `parentOrderId` when calling `SubmitOrder` to make future payments using the card information previously saved by the bank.

  ```csharp
  var result = await client.SaveCardForRecurringPayments(new SaveCardForRecurringPaymentsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **SaveCardForAutomaticPayments** \
  You must call this method right after calling `SubmitOrder`, before you navigate the user to the payment page to tell the bank you want to save user's card for future payments.

  You will be able to use the supplied `bank-order-id` paramter as a `ParentOrderId` property when calling `SubmitSubscriptionPaymentOrder` method.

  ```csharp
  var result = await client.SaveCardForAutomaticPayments(new SaveCardForAutomaticPaymentsRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **DeleteSavedCard** \
  Remove a previously saved card from the system.

  ```csharp
  var result = await client.DeleteSavedCard(new DeleteSavedCardRequest
  {
      OrderId = "bank-order-id"
  });
  ```

* **SubmitSubscriptionPaymentOrder** \
  Make automatic subscriptino payment using the card informatino saved as part of `SubmitOrder` and `SaveCardForAutomaticPayments` operations.

  ```csharp
  var result = await client.SubmitSubscriptionPaymentOrder(new SubmitSubscriptionPaymentOrderRequest
  {
      ParentOrderId = "bank-order-id"
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
