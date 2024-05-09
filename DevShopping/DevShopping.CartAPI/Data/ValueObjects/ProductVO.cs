namespace DevShopping.CartAPI.Data.ValueObjects;

public record ProductVO 
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? CategoryName { get; set; }

    public string? ImageURL { get; set; }
}
