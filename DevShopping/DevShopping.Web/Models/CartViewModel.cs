namespace DevShopping.Web.Models;

public record CartViewModel
{
    public virtual CartHeaderViewModel CartHeader { get; set; } = default!;
    public virtual IEnumerable<CartDetailViewModel> CartDetails { get; set; }
}
