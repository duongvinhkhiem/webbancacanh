using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;

        public AdminHomeController(IOrderService orderService, IProductService productService, ICategoryService categoryService, INewsService newsService)
        {
            _orderService = orderService;
            _productService = productService;
            _categoryService = categoryService;
            _newsService = newsService;
        }

        // GET: Admin/Home
        public async Task<ActionResult> Index()
        {
            // Lấy doanh thu từ cơ sở dữ liệu
            decimal revenueToday = await _orderService.GetRevenueByDayAsync(DateTime.Today);
            decimal revenueThisMonth = await _orderService.GetRevenueByMonthAsync(DateTime.Today.Year, DateTime.Today.Month);
            Dictionary<int, decimal> revenueByYear = await _orderService.GetRevenueByYearAsync(DateTime.Today.Year);

            // Lấy số lượng sản phẩm, danh mục, đơn hàng và tin tức
            int productCount = await _productService.GetProductCountAsync();
            int categoryCount = await _categoryService.GetCategoryCountAsync();
            int orderCount = await _orderService.GetOrderCountAsync();
            int newsCount = await _newsService.GetNewsCountAsync();

            // Truyền dữ liệu cho ViewBag
            ViewBag.RevenueToday = revenueToday;
            ViewBag.RevenueThisMonth = revenueThisMonth;
            ViewBag.RevenueByYear = revenueByYear;
            ViewBag.ProductCount = productCount;
            ViewBag.CategoryCount = categoryCount;
            ViewBag.OrderCount = orderCount;
            ViewBag.NewsCount = newsCount;

            return View();
        }
    }
}