namespace MRAYAQ.Domain.Models;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Tag,
    string ImageUrl,
    bool IsFeatured,
    Guid CategoryId,
    string CategoryName
);

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string Tag,
    string ImageUrl,
    bool IsFeatured,
    Guid CategoryId
);

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string Tag,
    string ImageUrl,
    bool IsFeatured,
    Guid CategoryId
);
