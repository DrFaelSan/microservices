using AutoMapper;
using DevShopping.ProductAPI.Data.ValueObjects;
using DevShopping.ProductAPI.Model;
using DevShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace DevShopping.ProductAPI.Repository;

public class ProductRepository : IProductRepository
{
    private readonly SQLServerContext _context;
    private IMapper _mapper;

    public ProductRepository(SQLServerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductVO>> FindAll()
    {
        IReadOnlyList<Product> products = await _context.Products.ToListAsync();
        return _mapper.Map<IReadOnlyList<ProductVO>>(products);
    }

    public async Task<ProductVO> FindById(long id)
    {
        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? new Product();
        return _mapper.Map<ProductVO>(product);
    }

    public async Task<ProductVO> Create(ProductVO vo)
    {
        Product product = _mapper.Map<Product>(vo);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(product);
    }

    public async Task<ProductVO> Update(ProductVO vo)
    {
        Product product = _mapper.Map<Product>(vo);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(product);
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id) ?? new Product();
            if (product.Id <= 0) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
