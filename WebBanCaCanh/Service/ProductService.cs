using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetRelativeProductsAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return null; // Hoặc có thể xử lý theo ý muốn của bạn
            }

            var relativeProducts = await _context.Products
                                                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != productId)
                                                .Take(4) // Lấy tối đa 4 sản phẩm
                                                .ToListAsync();

            return relativeProducts;
        }
        public async Task<int> GetProductCountAsync()
        {
            return await _context.Products.CountAsync();
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.ProductImages).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<List<Product>> SearchProductsAsync(string query)
        {
            // Sử dụng phương thức Where để lọc các sản phẩm có tên chứa query được nhập vào
            var searchResults = await _context.Products
                                              .Where(p => p.ProductName.Contains(query))
                                              .ToListAsync();

            return searchResults;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> AddProductImageAsync(ProductImage productImage)
        {
            _context.ProductImages.Add(productImage);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductImageAsync(int imageId)
        {
            try
            {
                var image = await _context.ProductImages.FindAsync(imageId);
                if (image == null)
                    return false;

                _context.ProductImages.Remove(image);
                var imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/" + image.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}