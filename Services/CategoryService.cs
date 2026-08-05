using DevOpsLab.Data;
using DevOpsLab.Dtos;
using DevOpsLab.Interfaces;
using DevOpsLab.Models;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(
    string? name,
    decimal? minPrice,
    decimal? maxPrice,
    int page,
    int pageSize)
{
    var query = _context.Categories.AsQueryable();

    if (!string.IsNullOrWhiteSpace(name))
    {
        query = query.Where(c =>
            c.Name.Contains(name));
    }

    query = query
        .Skip((page - 1) * pageSize)
        .Take(pageSize);

    return await query
        .Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        })
        .ToListAsync();
}

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        category.Name = dto.Name;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return true;
    }
}