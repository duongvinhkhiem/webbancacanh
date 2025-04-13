using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<int> GetOrderCountAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<int> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<decimal> GetRevenueByDayAsync(DateTime date);
        Task<decimal> GetRevenueByMonthAsync(int year, int month);
        Task<Dictionary<int, decimal>> GetRevenueByYearAsync(int year);
    }
}
