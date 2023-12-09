using MultiTenancy.Models;

namespace MultiTenancy.Services
{
    public interface IProductServices
    {
        Task<Product> AddProduct(Product product);
        Task<Product?> GetBuyId(int Id);
        Task<IReadOnlyList<Product>> GetAllProducts();
    }
}
