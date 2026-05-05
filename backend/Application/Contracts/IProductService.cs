using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Contracts;

public interface IProductService
{
    Task<List<ProductDto>> GetAsync(string? search, Guid? categoryId, decimal? minPrice, decimal? maxPrice, CancellationToken cancellationToken = default);
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
