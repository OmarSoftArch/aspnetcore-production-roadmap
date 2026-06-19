namespace LearningStore.Models
{
    public class OrderItem
    {
        private int _quantity;

        public OrderItem(Product product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity;
            UnitPrice = product.Price;
        }

        public Product Product { get; }

        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.", nameof(value));

                _quantity = value;
            }
        }

        public decimal UnitPrice { get; }
        public decimal LineTotal => UnitPrice * Quantity;

        public string GetSummary()
        {
            return $"{Product.Name} x {Quantity} = {LineTotal:C}";
        }
    }
}
