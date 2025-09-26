# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET class library project for the Bank of Georgia Aggregator E-commerce Client. It appears to be a payment gateway client library for integrating with Bank of Georgia's e-commerce aggregator API, similar to the iPay client mentioned in the README but for a different Bank of Georgia service.

## Build and Development Commands

### Build
```bash
# Build the solution
dotnet build src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln

# Build in Release mode
dotnet build src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln -c Release
```

### Clean
```bash
dotnet clean src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln
```

### Restore packages
```bash
dotnet restore src/Solution/BankOfGeorgiaAggregatorEcommerceClientSolution.sln
```

## Project Structure

The solution follows a standard .NET library structure:
- **src/Solution/** - Contains the Visual Studio solution and project
- **src/Solution/BankOfGeorgia.AggregatorEcommerceClient/** - Main library project
  - **Configuration/** - Contains options classes and DI extension methods
  - **BankOfGeorgiaAggregatorEcommerceClient.cs** - Main client interface and implementation

## Key Components

### Client Interface
- `IBankOfGeorgiaAggregatorEcommerceClient` - Main interface for the client
- `BankOfGeorgiaAggregatorEcommerceClient` - Internal implementation class

### Configuration
- `BankOfGeorgiaAggregatorEcommerceClientOptions` - Options class with ClientId and ClientSecret properties
- `IConfigurationExtentions` - Extension methods for ASP.NET Core DI integration via `AddBankOfGeorgiaAggregatorEcommergeClient`

## Target Frameworks
The library targets:
- .NET 7.0
- .NET 8.0

## Important Notes
- This is a class library project, not an executable application
- The library is designed to be consumed by ASP.NET Core applications through dependency injection
- The implementation is currently skeletal - the client interface and implementation are empty stubs
- Always use discriptive names for variables and methods
- Do not add comments, generate code which is easily read and understood. Use comments only for special cases where a context or a decision cannot be inferred just be reading the code when there is an external factor to it.
- Inverse conditinal statements and exist early where possible