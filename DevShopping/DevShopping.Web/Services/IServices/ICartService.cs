﻿using DevShopping.Web.Models;

namespace DevShopping.Web.Services.IServices;

public interface ICartService
{
    Task<CartViewModel> FindByCartUserId(string userId, string token);
    Task<CartViewModel> AddItemToCart(CartViewModel cart, string token);
    Task<CartViewModel> UpdateCart(CartViewModel cart, string token);
    Task<bool> RemoveFromCart(long id, string token);
    Task<bool> ApplyCoupon(CartViewModel cart, string couponCode, string token);
    Task<bool> RemoveCoupon(string userId, string token);
    Task<bool> ClearCart(string userId, string token);
    Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token);
    
}
