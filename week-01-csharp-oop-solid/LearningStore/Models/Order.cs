using LearningStore.Interfaces;

namespace LearningStore.Models
{
    public class Order
    {
        private readonly List<OrderItem> _items = [];

        public Order(int id, Customer customer, decimal taxRate)
        {
            if (id <= 0)
                throw new ArgumentException("Order id must be greater than zero.", nameof(id));

            if (taxRate < 0)
                throw new ArgumentException("Tax rate cannot be negative.", nameof(taxRate));

            Id = id;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            TaxRate = taxRate;
        }

        public int Id { get; }
        public Customer Customer { get; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public OrderStatus Status { get; private set; } = OrderStatus.Draft;
        public decimal TaxRate { get; }
        public IReadOnlyList<OrderItem> Items => _items;

        public decimal Subtotal => _items.Sum(item => item.LineTotal);
        public decimal TaxAmount => Subtotal * TaxRate;
        public decimal ShippingTotal => _items.Sum(item =>
            item.Product is IShippable shippableProduct
                ? shippableProduct.ShippingCost * item.Quantity
                : 0);

        public decimal GrandTotal => Subtotal + TaxAmount + ShippingTotal;

        public void AddItem(Product product, int quantity)
        {
            _items.Add(new OrderItem(product, quantity));
        }

        public void Confirm()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot confirm an order without items.");

            Status = OrderStatus.Confirmed;
        }

        public string GetSummary()
        {
            return $"Order #{Id} for {Customer.CompanyName} | Items: {Items.Count} | Total: {GrandTotal:C} | Status: {Status}";
        }
    }
}
