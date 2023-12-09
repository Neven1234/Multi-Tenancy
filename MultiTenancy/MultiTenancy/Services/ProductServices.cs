using Microsoft.EntityFrameworkCore;
using MultiTenancy.Data;
using MultiTenancy.Models;

namespace MultiTenancy.Services
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbcontext _Dbcontext;

        public ProductServices(AppDbcontext appDbcontext)
        {
            this._Dbcontext = appDbcontext;
        }

        public async Task<Product> AddProduct(Product product)
        {
           _Dbcontext.Products.Add(product);
           await _Dbcontext.SaveChangesAsync();
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetAllProducts()
        {
           return await _Dbcontext.Products.ToListAsync();
        }

        public async Task<Product?> GetBuyId(int Id)
        {
            var product= await _Dbcontext.Products.FindAsync(Id);
            return product;
        }
    }
}
