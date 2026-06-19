using LearningStore.DTOs;
using LearningStore.Interfaces;
using LearningStore.Models;
using LearningStore.Services;

decimal taxRate = 0.15m;

List<Product> products =
[
    new PhysicalProduct
    {
        Id = 1,
        Name = "Laptop",
        Price = 5000m,
        WeightInKg = 2.5m,
        ShippingCost = 50m
    },
    new DigitalProduct
    {
        Id = 2,
        Name = "C# Guide",
        Price = 99m,
        DownloadUrl = "https://learningstore.com/download/csharp-guide"
    },
    new DigitalProduct
    {
        Id = 3,
        Name = "ERP SaaS Monthly Subscription",
        Price = 1200m,
        DownloadUrl = "https://erp.example.com/onboarding"
    },
    new PhysicalProduct
    {
        Id = 4,
        Name = "Warehouse Barcode Scanner",
        Price = 850m,
        WeightInKg = 0.8m,
        ShippingCost = 35m
    }
];

Console.WriteLine("=== Products (Inheritance + Polymorphism) ===");
foreach (Product product in products)
    Console.WriteLine(product.GetSummary());

Console.WriteLine("\n=== Shipping (Interface Abstraction) ===");
foreach (IShippable shippableProduct in products.OfType<IShippable>())
    Console.WriteLine($"Shipping total with tax: {shippableProduct.GetTotalCost(taxRate):C}");

List<Customer> customers =
[
    new Customer
    {
        Id = 1,
        CompanyName = "Riyadh Retail Company",
        ContactEmail = "accounting@riyadh-retail.example"
    }
];

CreateOrderRequest request = new(
    CustomerId: 1,
    TaxRate: taxRate,
    Items:
    [
        new CreateOrderItemRequest(ProductId: 3, Quantity: 5),
        new CreateOrderItemRequest(ProductId: 4, Quantity: 2)
    ]);

IOrderService orderService = new OrderService(customers, products);
Order order = orderService.CreateOrder(request);

Console.WriteLine("\n=== Sales Order (DTO + Validation + Service) ===");
Console.WriteLine(order.GetSummary());
Console.WriteLine($"Customer: {order.Customer.GetDisplayName()}");
Console.WriteLine("Items:");

foreach (OrderItem item in order.Items)
    Console.WriteLine($"- {item.GetSummary()}");

Console.WriteLine($"Subtotal: {order.Subtotal:C}");
Console.WriteLine($"Tax: {order.TaxAmount:C}");
Console.WriteLine($"Shipping: {order.ShippingTotal:C}");
Console.WriteLine($"Grand Total: {order.GrandTotal:C}");
