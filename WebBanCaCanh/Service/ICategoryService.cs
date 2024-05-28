using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesWithProductsAsync();
        Task<int> GetCategoryCountAsync();
        Task<Category> GetCategoryWithProductsByCategoryIdAsync(int? categoryId);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}