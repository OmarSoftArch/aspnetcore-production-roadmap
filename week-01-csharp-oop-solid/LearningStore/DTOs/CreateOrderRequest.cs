namespace LearningStore.DTOs
{
    public record CreateOrderRequest(
        int CustomerId,
        decimal TaxRate,
        List<CreateOrderItemRequest> Items);
}
