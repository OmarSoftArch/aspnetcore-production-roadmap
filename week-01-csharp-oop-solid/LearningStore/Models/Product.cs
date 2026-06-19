namespace LearningStore.Models
{
    public abstract class Product
    {
        private string _name = string.Empty;
        private decimal _price;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name is required.", nameof(value));

                _name = value.Trim();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.", nameof(value));

                _price = value;
            }
        }

        public decimal GetPriceWithTax(decimal taxRate)
        {
            if (taxRate < 0)
                throw new ArgumentException("Tax rate cannot be negative.", nameof(taxRate));

            return Price * (1 + taxRate);
        }

        public virtual string GetSummary()
        {
            return $"#{Id} {Name} - {Price:C}";
        }
    }
}
