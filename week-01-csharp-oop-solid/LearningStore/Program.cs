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

Console.WriteLine("=== Physical Product ===");
Console.WriteLine($"{laptop.Name} - Total: {laptop.GetTotalCost(taxRate):C}");

Console.WriteLine("\n=== Digital Product ===");
Console.WriteLine($"{ebook.Name} - Price with Tax: {ebook.GetPriceWithTax(taxRate):C}");
Console.WriteLine($"Instant Delivery: {ebook.IsInstantDelivery}");