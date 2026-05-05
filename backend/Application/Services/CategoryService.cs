using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categories;

    public CategoryService(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public async Task<List<CategoryDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        var items = await _categories.GetAllAsync(cancellationToken);
        return items.Select(x => new CategoryDto(x.Id, x.Name, x.Slug)).ToList();
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category
        {
            Name = request.Name.Trim(),
            Slug = request.Slug.Trim()
        };

        await _categories.AddAsync(category, cancellationToken);
        return new CategoryDto(category.Id, category.Name, category.Slug);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return null;
        }

        category.Name = request.Name.Trim();
        category.Slug = request.Slug.Trim();

        await _categories.UpdateAsync(category, cancellationToken);
        return new CategoryDto(category.Id, category.Name, category.Slug);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return false;
        }

        await _categories.DeleteAsync(category, cancellationToken);
        return true;
    }
}
