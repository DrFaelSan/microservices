namespace DevShopping.CartAPI.Model;

public class Cart
{
    public virtual CartHeader CartHeader { get; set; } = default!;
    public virtual IEnumerable<CartDetail>  CartDetails { get; set; } = Enumerable.Empty<CartDetail>();
}
