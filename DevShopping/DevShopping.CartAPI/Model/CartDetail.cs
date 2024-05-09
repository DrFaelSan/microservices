using DevShopping.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShopping.CartAPI.Model;

[Table("cart_detail")]
public class CartDetail : BaseEntity
{
    [Column("count")]
    public int Count { get; set; }

    public long CartHeaderId { get; set; }
    
    [ForeignKey("CartHeaderId")]
    public virtual CartHeader CartHeader { get; set; } = default!;

    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; } = default;


}
