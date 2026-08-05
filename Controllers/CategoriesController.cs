using Microsoft.AspNetCore.Mvc;
using DevOpsLab.Interfaces;
using DevOpsLab.Dtos;

namespace DevOpsLab.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }


    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? name,
        decimal? minPrice,
        decimal? maxPrice,
        int page = 1,
        int pageSize = 10)
    {
        var categories = await _categoryService.GetAllAsync(
            name,
            minPrice,
            maxPrice,
            page,
            pageSize
        );

        return Ok(categories);
    }


    // GET: api/categories/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }


    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = await _categoryService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category
        );
    }


    // PUT: api/categories/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryDto dto)
    {
        var updated = await _categoryService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }


    // DELETE: api/categories/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}