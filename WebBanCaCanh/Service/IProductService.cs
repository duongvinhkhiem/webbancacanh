using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetRelativeProductsAsync(int productId);
        Task<List<Product>> GetAllProductsAsync();
        Task<int> GetProductCountAsync();
        Task<List<Product>> SearchProductsAsync(string query);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> AddProductImageAsync(ProductImage productImage);
        Task<bool> DeleteProductImageAsync(int imageId);
    }
}