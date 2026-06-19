using LearningStore.DTOs;
using LearningStore.Models;

namespace LearningStore.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderRequest request);
    }
}
