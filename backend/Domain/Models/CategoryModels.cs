namespace MRAYAQ.Domain.Models;

public record CategoryDto(Guid Id, string Name, string Slug);

public record CreateCategoryRequest(string Name, string Slug);

public record UpdateCategoryRequest(string Name, string Slug);
