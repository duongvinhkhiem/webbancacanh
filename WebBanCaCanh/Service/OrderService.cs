using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Service
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<int> AddOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return order.OrderId; // Return the ID of the newly added Order
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return 0;
            }
        }


        public async Task<bool> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                    return false;

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return false;
            }
        }
        public async Task<int> GetOrderCountAsync()
        {
            return await _context.Orders.CountAsync();
        }
        public async Task<decimal> GetRevenueByDayAsync(DateTime date)
        {
            try
            {
                // Tính tổng doanh thu từ các đơn hàng trong ngày được chỉ định
                decimal revenue = await _context.Orders
                                                .Where(o => DbFunctions.TruncateTime(o.OrderDate) == date.Date)
                                                .Select(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                                                .FirstOrDefaultAsync();

                return revenue;
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return 0;
            }
        }

        public async Task<decimal> GetRevenueByMonthAsync(int year, int month)
        {
            // Tính tổng doanh thu từ các đơn hàng trong tháng được chỉ định
            decimal revenue = await _context.Orders
                                            .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month)
                                            .Select(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                                            .DefaultIfEmpty(0)
                                            .SumAsync();
            return revenue;
        }
        public async Task<Dictionary<int, decimal>> GetRevenueByYearAsync(int year)
        {
            // Tính tổng doanh thu từ các đơn hàng trong mỗi tháng của năm được chỉ định
            Dictionary<int, decimal> revenueByMonth = await _context.Orders
                                                                    .Where(o => o.OrderDate.Year == year)
                                                                    .GroupBy(o => o.OrderDate.Month)
                                                                    .Select(g => new
                                                                    {
                                                                        Month = g.Key,
                                                                        Revenue = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                                                                    })
                                                                    .ToDictionaryAsync(x => x.Month, x => x.Revenue);
            return revenueByMonth;
        }


    }
}