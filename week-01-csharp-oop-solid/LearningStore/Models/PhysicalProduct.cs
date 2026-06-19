using LearningStore.Interfaces;

namespace LearningStore.Models
{
    public class PhysicalProduct : Product, IShippable
    {
        private decimal _weightInKg;
        private decimal _shippingCost;

        public decimal WeightInKg
        {
            get => _weightInKg;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Weight cannot be negative.", nameof(value));

                _weightInKg = value;
            }
        }

        public decimal ShippingCost
        {
            get => _shippingCost;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Shipping cost cannot be negative.", nameof(value));

                _shippingCost = value;
            }
        }

        public decimal GetTotalCost(decimal taxRate)
        {
            return GetPriceWithTax(taxRate) + ShippingCost;
        }

        public override string GetSummary()
        {
            return $"{base.GetSummary()} | Physical | Weight: {WeightInKg}kg | Shipping: {ShippingCost:C}";
        }
    }
}
