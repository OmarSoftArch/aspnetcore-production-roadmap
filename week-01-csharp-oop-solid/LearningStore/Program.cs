using LearningStore.Interfaces;
using LearningStore.Models;

var laptop = new PhysicalProduct
{
    Name = "Laptop",
    Price = 5000m,
    WeightInKg = 2.5m,
    ShippingCost = 50m
};

var ebook = new DigitalProduct
{
    Name = "C# Guide",
    Price = 99m,
    DownloadUrl = "https://learningstore.com/download/csharp-guide"
};

decimal taxRate = 0.15m;

// Polymorphism — قائمة واحدة لكل المنتجات
List<Product> cart = { laptop, ebook };

Console.WriteLine("=== Cart Summary (Polymorphism) ===");
foreach (Product item in cart)
    Console.WriteLine(item.GetSummary());

// Abstraction — IShippable
Console.WriteLine("\n=== Shipping (Abstraction) ===");
IShippable shippable = laptop;
Console.WriteLine($"Shippable total: {shippable.GetTotalCost(taxRate):C}");