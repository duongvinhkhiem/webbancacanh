using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public class ProductPromotionService : IProductPromotionService
    {
        private readonly ApplicationDbContext _context;

        public ProductPromotionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductPromotion>> GetAllProductPromotionsAsync()
        {
            return await _context.ProductPromotions.ToListAsync();
        }

        public async Task<ProductPromotion> GetProductPromotionByIdAsync(int id)
        {
            return await _context.ProductPromotions.FindAsync(id);
        }

        public async Task<bool> AddProductPromotionAsync(ProductPromotion productPromotion)
        {
            try
            {
                _context.ProductPromotions.Add(productPromotion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exception if needed
                return false;
            }
        }

        public async Task<bool> ApplyPromotionToProductAsync(int productId, int promotionId)
        {
            try
            {
                // Check if the product already has this promotion applied
                var existingPromotion = await _context.ProductPromotions.FirstOrDefaultAsync(pp => pp.ProductId == productId && pp.PromotionId == promotionId);
                if (existingPromotion != null)
                {
             
                }
                else
                {
                    // Create a new product promotion entry
                    var newPromotion = new ProductPromotion
                    {
                        ProductId = productId,
                        PromotionId = promotionId
                     
                    };
                    _context.ProductPromotions.Add(newPromotion);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exception if needed
                return false;
            }
        }

        public async Task<bool> RemovePromotionFromProductAsync(int productId, int promotionId)
        {
            try
            {
                // Find the product promotion entry
                var productPromotion = await _context.ProductPromotions.FirstOrDefaultAsync(pp => pp.ProductId == productId && pp.PromotionId == promotionId);
                if (productPromotion != null)
                {
                    // Remove the product promotion entry
                    _context.ProductPromotions.Remove(productPromotion);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // No product promotion found
            }
            catch (Exception)
            {
                // Handle exception if needed
                return false;
            }
        }

        public async Task<bool> UpdateProductPromotionAsync(ProductPromotion productPromotion)
        {
            try
            {
                _context.Entry(productPromotion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exception if needed
                return false;
            }
        }
    }
}