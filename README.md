# Uber.Direct.DaaS Client

A minimal, strongly-typed **C# client library** for the [Uber Direct (DaaS) REST API](https://developer.uber.com/docs/deliveries/api-reference/daas).  

This library wraps the raw HTTP endpoints into clean async methods and request/response DTOs, making it easier to integrate **on-demand delivery** capabilities into .NET applications.

---

## ✨ Features

- 🔒 **OAuth2 Bearer authentication** (access token supplied to client).  
- 📦 Coverage of all primary DaaS endpoints:
  - Create Quote  
  - Create Delivery  
  - List Deliveries  
  - Update Delivery  
  - Get Delivery  
  - Cancel Delivery  
  - Proof of Delivery (Base64 image)  
- 📑 DTOs with `[JsonPropertyName]` attributes to ensure snake_case serialization.  
- `BuildStructuredAddressJson` helper for structured, JSON-escaped addresses (required in some regions).  
- Unit tests with a lightweight in-memory HTTP handler.

---

## 📚 Project Purpose

Uber Direct (Delivery as a Service, **DaaS**) exposes a REST API for merchants and developers who need to **offer same-day or on-demand delivery** to their customers.  

This project’s goal is to:

1. Provide a **minimal yet production-ready C# client** that maps directly to Uber’s API.  
2. Demonstrate **robust request building and response parsing** in .NET.  
3. Serve as a **teaching and testing scaffold** for teams adopting Uber Direct in their backend services.  

---

## 🚀 Getting Started

### 1. Install

Clone this repository and add the `Uber.Direct.DaaS` project to your solution.

### 2. Usage

```csharp
using System;
using System.Net.Http;
using Uber.Direct.DaaS;

var http = new HttpClient();
var client = new UberDirectClient(http, "https://api.uber.com", "<CUSTOMER_ID>")
{
    AccessToken = "<ACCESS_TOKEN>" // OAuth2 token with scope eats.deliveries
};

// Create a quote
var quote = await client.CreateQuoteAsync(new CreateQuoteRequest
{
    pickup_address = UberDirectClient.BuildStructuredAddressJson(
        "20 W 34th St", "Floor 2", "New York", "NY", "10001"),
    dropoff_address = UberDirectClient.BuildStructuredAddressJson(
        "285 Fulton St", "", "New York", "NY", "10006")
});

// Use the quote to create a delivery
var delivery = await client.CreateDeliveryAsync(new CreateDeliveryRequest
{
    quote_id = quote.id,
    pickup_address = "...",
    pickup_name = "My Store",
    pickup_phone_number = "1111111111",
    dropoff_address = "...",
    dropoff_name = "Customer Name",
    dropoff_phone_number = "2222222222",
    manifest_items = new List<ManifestItem>
    {
        new ManifestItem { name = "Box", quantity = 1 }
    }
});

Console.WriteLine($"Delivery created: {delivery.id}, status={delivery.status}");
```

---

## 🧪 Testing

Unit tests are located in the `Tests` project. They:

- Stub HTTP responses with a custom in-memory handler.  
- Verify correct methods, paths, headers, query params, and body serialization.  
- Validate JSON deserialization into DTOs.  
- Assert that non-2xx responses throw `HttpRequestException`.  

Run tests with:

```bash
dotnet test
```

---

## 🔧 Requirements

- .NET 6.0 or later  
- A valid Uber Direct **Customer ID** and OAuth2 **access token** with scope `eats.deliveries`

---

## 📄 License

This project is released under the MIT License. See `LICENSE` for details.
