using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningStore.Models
{
    public class Product
    {

        private decimal _price;
        public string Name { get; set; } = string.Empty;
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
            return $"{Name} - {Price:C}";
        }
    }
}
