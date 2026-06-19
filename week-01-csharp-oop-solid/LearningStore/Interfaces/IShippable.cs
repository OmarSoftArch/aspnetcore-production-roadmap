namespace LearningStore.Interfaces
{
    public interface IShippable
    {
        decimal ShippingCost { get; }
        decimal GetTotalCost(decimal taxRate);
    }
}
