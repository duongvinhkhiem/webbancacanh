using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface INewsService
    {
        Task<List<News>> GetAllNewsAsync();
        Task<int> GetNewsCountAsync();
        Task<News> GetNewsByIdAsync(int id);
        Task<bool> AddNewsAsync(News news);
        Task<bool> UpdateNewsAsync(News news);
        Task<bool> DeleteNewsAsync(int id);
    }
}
