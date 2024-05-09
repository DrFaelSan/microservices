namespace DevShopping.Web.Models;

public record CartHeaderViewModel 
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CouponCode { get; set; } 
    public double PurchaseAmount { get; set; }
}
