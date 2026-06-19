using LearningStore.DTOs;
using LearningStore.Interfaces;
using LearningStore.Models;
using LearningStore.Validation;

namespace LearningStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IReadOnlyList<Customer> _customers;
        private readonly IReadOnlyList<Product> _products;
        private readonly CreateOrderRequestValidator _validator = new();
        private int _nextOrderId = 1;

        public OrderService(IReadOnlyList<Customer> customers, IReadOnlyList<Product> products)
        {
            _customers = customers;
            _products = products;
        }

        public Order CreateOrder(CreateOrderRequest request)
        {
            List<string> validationErrors = _validator.Validate(request);
            if (validationErrors.Count > 0)
                throw new InvalidOperationException(string.Join(Environment.NewLine, validationErrors));

            Customer customer = FindActiveCustomer(request.CustomerId);
            Order order = new(_nextOrderId++, customer, request.TaxRate);

            foreach (CreateOrderItemRequest item in request.Items)
            {
                Product product = FindProduct(item.ProductId);
                order.AddItem(product, item.Quantity);
            }

            order.Confirm();
            return order;
        }

        private Customer FindActiveCustomer(int customerId)
        {
            Customer? customer = _customers.FirstOrDefault(customer => customer.Id == customerId);

            if (customer is null)
                throw new InvalidOperationException($"Customer #{customerId} was not found.");

            if (!customer.IsActive)
                throw new InvalidOperationException($"Customer #{customerId} is inactive.");

            return customer;
        }

        private Product FindProduct(int productId)
        {
            return _products.FirstOrDefault(product => product.Id == productId)
                ?? throw new InvalidOperationException($"Product #{productId} was not found.");
        }
    }
}
