using Microsoft.AspNetCore.Mvc;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Services;

namespace SalesInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<CustomerResponse> customers = await _customerService.GetAllAsync(cancellationToken);

        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        CustomerResponse? customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (await _customerService.EmailExistsAsync(request.Email, null, cancellationToken))
            return BadRequest(new { message = "Email is already used by another customer." });

        CustomerResponse customer = await _customerService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> Update(int id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (await _customerService.EmailExistsAsync(request.Email, id, cancellationToken))
            return BadRequest(new { message = "Email is already used by another customer." });

        CustomerResponse? customer = await _customerService.UpdateAsync(id, request, cancellationToken);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _customerService.DeleteAsync(id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
