#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreakyFashionServices.CatalogService.Data;
using FreakyFashionServices.CatalogService.Models.Domain;
using FreakyFashionServices.CatalogService.Models.DTO;

namespace FreakyFashionServices.CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CatalogServiceContext _context;

        public ProductsController(CatalogServiceContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public IEnumerable<ProductDto> GetProducts()
        {
            var productDtos = _context.Products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price,
                ArticleNumber = x.ArticleNumber,
                UrlSlug = x.UrlSlug,
            });
            
            return productDtos;
        }

        // POST: api/products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostProduct(AddProductDto addProductDto)
        {
            var product = new Product(
                addProductDto.Name,
                addProductDto.Description,
                addProductDto.ImageUrl,
                addProductDto.Price,
                addProductDto.ArticleNumber);
            
            _context.Products.Add(product);
            _context.SaveChanges();

            return Created("", product);
        }
    }
}
