﻿using DevShopping.Web.Models;
using DevShopping.Web.Services.IServices;
using DevShopping.Web.Utils;
using System.Net.Http.Headers;

namespace DevShopping.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient client)
        =>  _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<CartViewModel> FindByCartUserId(string userId, string token)
    {
        SetBearerToken(token);
        var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
        return await response.ReadContentAs<CartViewModel>();
    }
    public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
    {
        SetBearerToken(token);
        var response = await _client.PostAsJson($"{BasePath}/add-cart", cart);
        await Console.Out.WriteLineAsync($"{response.Content}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        else throw new Exception("Something went wrong when calling API");
    }
    public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
    {
        SetBearerToken(token);
        var response = await _client.PutAsJson($"{BasePath}/update-cart", cart);
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>();
        else throw new Exception("Something went wrong when calling API");
    }
    public async Task<bool> RemoveFromCart(long cartId, string token)
    {
        SetBearerToken(token);
        var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
        if (response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>();
        else throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCoupon(CartViewModel cart, string couponCode, string token)
    {
        throw new NotImplementedException();
    }
    public async Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> ClearCart(string userId, string token)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> RemoveCoupon(string userId, string token)
    {
        throw new NotImplementedException();
    }
    private void SetBearerToken(string token)
       => _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
}
