using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningStore.Interfaces
{
    interface IShippable
    {
        decimal ShippingCost { get; }
        decimal GetTotalCost(decimal taxRate);
    }
}
