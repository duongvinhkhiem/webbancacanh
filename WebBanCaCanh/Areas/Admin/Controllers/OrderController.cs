using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ cho phép người dùng có vai trò "Admin" truy cập
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService; // Dịch vụ đơn hàng

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Order
        public async Task<ActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync(); // Lấy tất cả đơn hàng
            return View(orders); // Trả về view với danh sách đơn hàng
        }

        // GET: Order/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); // Trả về lỗi nếu id không hợp lệ
            }

            var order = await _orderService.GetOrderByIdAsync(id.Value); // Lấy thông tin đơn hàng theo id
            if (order == null)
            {
                return HttpNotFound(); // Trả về lỗi nếu không tìm thấy đơn hàng
            }

            return View(order); // Trả về view với thông tin đơn hàng
        }
    }
}
