using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningStore.Models
{
    class PhysicalProduct:Product
    {
         public decimal WeightInKg { get; set; }
    public decimal ShippingCost { get; set; }
    
    public decimal GetTotalCost(decimal taxRate)
    {
        return GetPriceWithTax(taxRate) + ShippingCost;
    }
    }
}
