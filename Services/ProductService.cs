using DevOpsLab.Data;
using DevOpsLab.Dtos;
using DevOpsLab.Interfaces;
using DevOpsLab.Models;
using Microsoft.EntityFrameworkCore;


namespace DevOpsLab.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(
    string? name,
    decimal? minPrice,
    decimal? maxPrice,
    int page,
    int pageSize)
{
    var query = _context.Products
    .Include(p => p.Category)
    .AsQueryable();

    // Filtro por nome
    if (!string.IsNullOrWhiteSpace(name))
    {
        query = query.Where(p =>
            p.Name.Contains(name));
    }

    // Filtro por preço mínimo
    if (minPrice.HasValue)
    {
        query = query.Where(p =>
            p.Price >= minPrice.Value);
    }

    // Filtro por preço máximo
    if (maxPrice.HasValue)
    {
        query = query.Where(p =>
            p.Price <= maxPrice.Value);
    }

    // Paginação
    query = query
        .Skip((page - 1) * pageSize)
        .Take(pageSize);

    return await query
    .Select(p => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        CategoryId = p.CategoryId,
        CategoryName = p.Category!.Name
    })
    .ToListAsync();
}

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category!.Name
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        _context.Products.Add(product);

        await _context.Entry(product)
            .Reference(p => p.Category)
            .LoadAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category!.Name
        };
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return true;
    }

    
}