﻿using DevShooping.ProductAPI.Data.ValueObjects;
using DevShooping.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevShooping.ProductAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(IProductRepository));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductVO>>> FindAll()
    {
        var products = await _repository.FindAll();
        if(!products.Any()) return NotFound();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductVO>> FindById(long id)
    {
        var product = await _repository.FindById(id);
        if(product.Id <= 0) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductVO>> Create([FromBody]ProductVO productVO)
    {
        if (productVO is null) return BadRequest();
        var product = await _repository.Create(productVO);
        return Ok(product);
    }

    [HttpPut]
    public async Task<ActionResult<ProductVO>> Update([FromBody]ProductVO productVO)
    {
        if (productVO is null) return BadRequest();
        var product = await _repository.Update(productVO);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(long id)
    {
        var status = await _repository.Delete(id);
        if (!status) return BadRequest();
        return Ok(status);
    }
}
