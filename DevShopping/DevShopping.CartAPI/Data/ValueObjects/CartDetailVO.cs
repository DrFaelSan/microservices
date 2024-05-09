namespace DevShopping.CartAPI.Data.ValueObjects;

public record CartDetailVO
{
    public long Id { get; set; }
    public int Count { get; set; }
    public long CartHeaderId { get; set; }
    public virtual CartHeaderVO CartHeader { get; set; } = default!;
    public long ProductId { get; set; }
    public virtual ProductVO Product{ get; set; } = default!;
}
