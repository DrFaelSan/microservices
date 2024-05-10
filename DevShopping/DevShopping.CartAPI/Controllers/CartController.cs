

using DevShopping.CartAPI.Data.ValueObjects;
using DevShopping.CartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
    private ICartRepository _repository;

    public CartController(ICartRepository repository)
        =>  _repository = repository;

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartVO>> FindById(string id)
    {
        var cart = await _repository.FindCartByUserIdAsync(id);
        if(cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
    {
        var cart = await _repository.SaveOrUpdateCartAsync(vo);
        if (cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
    {
        var cart = await _repository.SaveOrUpdateCartAsync(vo);
        if (cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<bool>> RemoveCart(int id)
    {
        var status = await _repository.RemoveFromCartAsync(id);
        if (!status) return BadRequest();
        return Ok(status);
    }
}
