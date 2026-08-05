using DevOpsLab.Dtos;


namespace DevOpsLab.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(
        string? name,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize);

    Task<CategoryDto?> GetByIdAsync(int id);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateCategoryDto dto);

    Task<bool> DeleteAsync(int id);
}