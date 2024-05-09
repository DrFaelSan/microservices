using DevShopping.CartAPI.Data.ValueObjects;

namespace DevShopping.CartAPI.Repository.Interfaces;

public interface ICartRepository
{
    Task<CartVO> FindCartByUserIdAsync(string userId);
    Task<CartVO> SaveOrUpdateCartAsync(CartVO cart);
    Task<bool> RemoveFromCartAsync(long cartDetailId);
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> RemoveCouponAsync(string userId);
    Task<bool> ClearCartAsync(string userId);
}
