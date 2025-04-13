using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllBannersAsync();
        Task<Banner> GetBannerByIdAsync(int id);
        Task<bool> AddBannerAsync(Banner banner);
        Task<bool> UpdateBannerAsync(Banner banner);
        Task<bool> DeleteBannerAsync(int id);
    }
}
