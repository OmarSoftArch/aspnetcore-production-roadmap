using Microsoft.AspNetCore.Mvc;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Services;

namespace SalesInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    // GET /api/products
    [HttpGet]
    public ActionResult<IReadOnlyList<ProductResponse>> GetAll()
    {
        IReadOnlyList<ProductResponse> products = _productService.GetAll();

        return Ok(products);
    }

    // GET /api/products/1
    [HttpGet("{id:int}")]
    public ActionResult<ProductResponse> GetById(int id)
    {
        ProductResponse? product = _productService.GetById(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    // POST /api/products
    [HttpPost]
    public ActionResult<ProductResponse> Create(CreateProductRequest request)
    {
        if (!_categoryService.Exists(request.CategoryId))
            return BadRequest(new { message = "CategoryId does not exist." });

        ProductResponse product = _productService.Create(request);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT /api/products/1
    [HttpPut("{id:int}")]
    public ActionResult<ProductResponse> Update(int id, UpdateProductRequest request)
    {
        if (!_productService.Exists(id))
            return NotFound();

        if (!_categoryService.Exists(request.CategoryId))
            return BadRequest(new { message = "CategoryId does not exist." });

        ProductResponse? product = _productService.Update(id, request);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    // DELETE /api/products/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _productService.Delete(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
