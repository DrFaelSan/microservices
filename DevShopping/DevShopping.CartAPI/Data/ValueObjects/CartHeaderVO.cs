namespace DevShopping.CartAPI.Data.ValueObjects;

public record CartHeaderVO 
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CouponCode { get; set; } 
}
