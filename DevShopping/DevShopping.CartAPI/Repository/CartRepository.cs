using AutoMapper;
using DevShopping.CartAPI.Data.ValueObjects;
using DevShopping.CartAPI.Model;
using DevShopping.CartAPI.Model.Context;
using DevShopping.CartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevShopping.CartAPI.Repository;

public class CartRepository : ICartRepository
{
    private readonly SQLServerContext _context;
    private IMapper _mapper;
    public CartRepository(SQLServerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
        if(cartHeader is not null)
        {
            _context.CartDetails.RemoveRange(
                _context.CartDetails.AsNoTracking()
                                    .Where(c => c.CartHeaderId == cartHeader.Id)
                );
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CartVO> FindCartByUserIdAsync(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId)
        };

        cart.CartDetails = await _context.CartDetails.Include(c => c.Product)
                                                     .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                                                     .ToListAsync();

        return _mapper.Map<CartVO>(cart);
    }

    public async Task<bool> RemoveCouponAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveFromCartAsync(long cartDetailId)
    {
        try
        {
            CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == cartDetailId);

            int total = _context.CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeaderId)
                                            .Count();

            _context.CartDetails.Remove(cartDetail);

            if(total == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                _context.CartHeaders.Remove(cartHeaderToRemove);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<CartVO> SaveOrUpdateCartAsync(CartVO vo)
    {
        Cart cart = _mapper.Map<Cart>(vo);

        long productId = vo.CartDetails.FirstOrDefault().ProductId;

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }

        var cartHeader = await _context.CartHeaders.AsNoTracking()
                                                   .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        if (cartHeader is null)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();

            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartDetails.FirstOrDefault().Product = null;
            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());

            await _context.SaveChangesAsync();
        }
        else
        {
            var cartDetail = await _context.CartDetails.AsNoTracking()
                                                       .FirstOrDefaultAsync(p => 
                                                            p.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                                                            p.CartHeaderId == cartHeader.Id);

            if (cartDetail is null) 
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
        }

        return _mapper.Map<CartVO>(cart);
    }
}
