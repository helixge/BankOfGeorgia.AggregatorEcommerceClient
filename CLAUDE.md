# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

.NET client library for Bank of Georgia's Aggregator E-commerce API. Provides OAuth2 authentication and payment order submission functionality for integrating with Bank of Georgia's payment gateway.

## Build and Development Commands

### Build
```bash
# Build the solution
dotnet build src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln

# Build in Release mode
dotnet build src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln -c Release
```

### Test
```bash
# Run all tests
dotnet test src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln

# Run tests with detailed output
dotnet test src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln --logger "console;verbosity=detailed"

# Run specific test
dotnet test src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln --filter "FullyQualifiedName~IntegrationTests"
```

### Clean
```bash
dotnet clean src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln
```

### Restore packages
```bash
dotnet restore src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln
```

## Architecture

### Authentication Flow
1. **IBankOfGeorgiaApiTokenClient** - Handles OAuth2 client credentials flow
   - Obtains access tokens from Bank of Georgia's OAuth endpoint
   - Uses Basic authentication with ClientId:ClientSecret
   - Default OAuth URL: `https://oauth2.bog.ge/auth/realms/bog/protocol/openid-connect/token`

2. **IBankOfGeorgiaAggregatorEcommerceClient** - Main client for API operations
   - Uses token from token client for authorization
   - Submits payment orders to the aggregator API
   - Default API base URL: `https://api.bog.ge/payments`

### Dependency Injection Setup
```csharp
services.AddBankOfGeorgiaAggregatorEcommerce(configuration);
// or with custom section
services.AddBankOfGeorgiaAggregatorEcommerce(configuration, "CustomSectionName");
```

Configuration expects:
```json
{
  "BankOfGeorgiaAggregatorEcommerce": {
    "ClientId": "required",
    "ClientSecret": "required",
    "OAuthUrl": "optional - uses default if not provided",
    "ApiBaseUrl": "optional - uses default if not provided"
  }
}
```

### Key Components

- **Authentication/** - OAuth token management
  - `BankOfGeorgiaApiTokenClient` - OAuth2 client credentials implementation
  - `TokenApiResponse` - Token response model
  - `BankOfGeorgiaApiTokenException` - Authentication-specific exceptions

- **Models/** - Request/response DTOs
  - `SubmitOrderRequest` - Main order submission payload
  - Supporting models: `PurchaseUnits`, `BasketItem`, `RedirectUrls`, `Buyer`, etc.
  - Enums: `ApplicationType`, `CaptureType`, `PaymentMethod`, `CardType`, `CampaignType`

- **Serialization/** - `BankOfGeorgiaApiSerializationService` for JSON handling

- **Configuration/** - DI setup and options validation

### Testing

- **Unit Tests**: Located in `BankOfGeorgia.AggregatorEcommerceClient.Tests`
- **Integration Tests**: Use `IntegrationTestBase` base class with full DI container
- Test framework: xUnit with NSubstitute for mocking
- Configuration: Uses User Secrets (ID: `945403e8-d305-4e24-a7b9-1a138a228127`)

## Target Framework
- .NET 8.0

## Code Guidelines
- Use descriptive names for variables and methods
- **NEVER add comments to code** - no inline comments, no block comments, no XML documentation comments
- Use early returns and guard clauses
- Follow existing patterns in the codebase
- Always and a trailing line break when creating a file and make sure there is one when editing it.
