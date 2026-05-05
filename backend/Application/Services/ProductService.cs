using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;

    public ProductService(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    public async Task<List<ProductDto>> GetAsync(string? search, Guid? categoryId, decimal? minPrice, decimal? maxPrice, CancellationToken cancellationToken = default)
    {
        var items = await _products.GetAsync(search, categoryId, minPrice, maxPrice, cancellationToken);
        return items.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _products.GetByIdAsync(id, cancellationToken);
        return item is null ? null : ToDto(item);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categories.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price,
            Tag = request.Tag.Trim(),
            ImageUrl = request.ImageUrl.Trim(),
            IsFeatured = request.IsFeatured,
            CategoryId = request.CategoryId
        };

        await _products.AddAsync(product, cancellationToken);
        product.Category = category;

        return ToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        var category = await _categories.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Price = request.Price;
        product.Tag = request.Tag.Trim();
        product.ImageUrl = request.ImageUrl.Trim();
        product.IsFeatured = request.IsFeatured;
        product.CategoryId = request.CategoryId;
        product.Category = category;

        await _products.UpdateAsync(product, cancellationToken);

        return ToDto(product);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        await _products.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static ProductDto ToDto(Product product)
    {
        var categoryName = product.Category?.Name ?? string.Empty;
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Tag,
            product.ImageUrl,
            product.IsFeatured,
            product.CategoryId,
            categoryName
        );
    }
}
