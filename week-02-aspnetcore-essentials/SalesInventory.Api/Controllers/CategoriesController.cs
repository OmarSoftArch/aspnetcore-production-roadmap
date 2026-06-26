using Microsoft.AspNetCore.Mvc;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Services;

namespace SalesInventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    // Constructor injection: ASP.NET Core injects the registered service from Program.cs.
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET /api/categories
    // Returns 200 OK with all categories.
    [HttpGet]
    public ActionResult<IReadOnlyList<CategoryResponse>> GetAll()
    {
        IReadOnlyList<CategoryResponse> categories = _categoryService.GetAll();

        return Ok(categories);
    }

    // GET /api/categories/1
    // Returns 200 OK when found, or 404 Not Found when the category does not exist.
    [HttpGet("{id:int}")]
    public ActionResult<CategoryResponse> GetById(int id)
    {
        CategoryResponse? category = _categoryService.GetById(id);

        if (category is null)
            return NotFound();

        return Ok(category);
    }

    // POST /api/categories
    // Returns 201 Created with the new category when the request is valid.
    [HttpPost]
    public ActionResult<CategoryResponse> Create(CreateCategoryRequest request)
    {
        CategoryResponse category = _categoryService.Create(request);

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // PUT /api/categories/1
    // Returns 200 OK with the updated category, or 404 Not Found when the category does not exist.
    [HttpPut("{id:int}")]
    public ActionResult<CategoryResponse> Update(int id, UpdateCategoryRequest request)
    {
        CategoryResponse? category = _categoryService.Update(id, request);

        if (category is null)
            return NotFound();

        return Ok(category);
    }

    // DELETE /api/categories/1
    // Returns 204 No Content when deleted, or 404 Not Found when the category does not exist.
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _categoryService.Delete(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
