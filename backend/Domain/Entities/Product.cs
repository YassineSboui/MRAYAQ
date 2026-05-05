namespace MRAYAQ.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Tag { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsFeatured { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }
}
