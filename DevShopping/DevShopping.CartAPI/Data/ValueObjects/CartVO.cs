namespace DevShopping.CartAPI.Data.ValueObjects;

public record CartVO
{
    public virtual CartHeaderVO CartHeader { get; set; } = default!;
    public virtual IEnumerable<CartDetailVO> CartDetails { get; set; }
}
