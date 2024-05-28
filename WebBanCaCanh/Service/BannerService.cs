using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;

        public BannerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Banner>> GetAllBannersAsync()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<Banner> GetBannerByIdAsync(int id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task<bool> AddBannerAsync(Banner banner)
        {
            try
            {
                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        public async Task<bool> UpdateBannerAsync(Banner banner)
        {
            try
            {
                _context.Entry(banner).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            try
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner == null)
                    return false;

                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }
    }
}