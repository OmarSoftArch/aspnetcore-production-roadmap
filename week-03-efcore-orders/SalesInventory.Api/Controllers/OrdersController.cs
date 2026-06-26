using Microsoft.AspNetCore.Mvc;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Services;

namespace SalesInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<OrderResponse> orders = await _orderService.GetAllAsync(cancellationToken);

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        OrderResponse? order = await _orderService.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        ServiceResult<OrderResponse> result = await _orderService.CreateAsync(request, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = result.Error });

        OrderResponse order = result.Value!;

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OrderResponse>> Update(int id, UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        ServiceResult<OrderResponse> result = await _orderService.UpdateAsync(id, request, cancellationToken);

        if (!result.Succeeded && result.Error == "Order was not found.")
            return NotFound();

        if (!result.Succeeded)
            return BadRequest(new { message = result.Error });

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _orderService.DeleteAsync(id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
