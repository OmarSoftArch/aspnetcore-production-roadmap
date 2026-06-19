using LearningStore.DTOs;

namespace LearningStore.Validation
{
    public class CreateOrderRequestValidator
    {
        public List<string> Validate(CreateOrderRequest request)
        {
            List<string> errors = [];

            if (request.CustomerId <= 0)
                errors.Add("CustomerId must be greater than zero.");

            if (request.TaxRate < 0)
                errors.Add("TaxRate cannot be negative.");

            if (request.Items.Count == 0)
                errors.Add("Order must contain at least one item.");

            foreach (CreateOrderItemRequest item in request.Items)
            {
                if (item.ProductId <= 0)
                    errors.Add("ProductId must be greater than zero.");

                if (item.Quantity <= 0)
                    errors.Add($"Quantity for product #{item.ProductId} must be greater than zero.");
            }

            return errors;
        }
    }
}
