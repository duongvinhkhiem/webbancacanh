using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface IProductPromotionService
    {
        Task<List<ProductPromotion>> GetAllProductPromotionsAsync();
        Task<ProductPromotion> GetProductPromotionByIdAsync(int id);
        Task<bool> AddProductPromotionAsync(ProductPromotion productPromotion);
        Task<bool> ApplyPromotionToProductAsync(int productId, int promotionId);
        Task<bool> RemovePromotionFromProductAsync(int productId, int promotionId);
        Task<bool> UpdateProductPromotionAsync(ProductPromotion productPromotion);
 
    }
}