using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;
using WebBanCaCanh.ViewModels;

namespace WebBanCaCanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IBannerService _bannerService;
        private readonly IProductService _productService;
        private readonly INewsService _newsService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        public HomeController(ICategoryService categoryService, IBannerService bannerService, IProductService productService, INewsService newsService, IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _categoryService = categoryService;
            _bannerService = bannerService;
            _productService = productService;
          _productService = productService;
            _newsService = newsService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;

        }
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());

                if (user != null && await userManager.IsInRoleAsync(user.Id, "Admin"))
                {
                    return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                }
            }
            var categories = await _categoryService.GetAllCategoriesWithProductsAsync();
            var banners = await _bannerService.GetAllBannersAsync();
            ViewBag.Banners = banners;
            return View(categories);
        }

        public async Task<ActionResult> Product(int? categoryId)
        {
            var category = await _categoryService.GetCategoryWithProductsByCategoryIdAsync(categoryId);
            var categories = await _categoryService.GetAllCategoriesWithProductsAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            return View(category);
        }
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }

            var relativeProducts = await _productService.GetRelativeProductsAsync(id.Value);
            ViewBag.RelativeProducts = relativeProducts;

            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<OrderDetail>();
            }

            var cart = (List<OrderDetail>)Session["Cart"];

            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new OrderDetail
                {
                    ProductId = productId,
                    Product = product, 
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                cart.Add(newItem);
            }

            Session["Cart"] = cart;
            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
           
            if (Session["Cart"] == null)
            {
                return View();
            }

            var cart = (List<OrderDetail>)Session["Cart"];
            if (cart.Count == 0)
            {
                return View();
            }

            return View(cart);
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            if (Session["Cart"] != null)
            {
                var cart = (List<OrderDetail>)Session["Cart"];
                var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);

                    Session["Cart"] = cart;
                }
            }

            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult UpdateCart(int productId, string action)
        {
            if (Session["Cart"] != null)
            {
                var cart = (List<OrderDetail>)Session["Cart"];
                var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == productId);

                if (itemToUpdate != null)
                {
                    if (action == "increase")
                    {
                        itemToUpdate.Quantity++;
                    }
                    else if (action == "decrease")
                    {
                        if (itemToUpdate.Quantity > 1)
                        {
                            itemToUpdate.Quantity--;
                        }
                    }

                    Session["Cart"] = cart;
                }
            }

            return RedirectToAction("Cart");
        }

        public async Task<ActionResult> News()
        {

            var news = await _newsService.GetAllNewsAsync();
            return View(news);
        }
        public async Task<ActionResult> NewsDetail(int id)
        {
         
            var news = await _newsService.GetNewsByIdAsync(id);
            return View(news); 
        }
       
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
        
                return RedirectToAction("Index");
            }

      
            var searchResults = await _productService.SearchProductsAsync(query);

     
            return View(searchResults);
        }
        public ActionResult Checkout()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Cart");
            }

            var cart = (List<OrderDetail>)Session["Cart"];

            // Chuyển hướng đến view Checkout để người dùng nhập thông tin đặt hàng
            return View("Checkout");
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                if (Session["Cart"] != null)
                {
                    var cart = (List<OrderDetail>)Session["Cart"];

                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Chờ xử lý",
                        UserId = user != null ? user.Id : null,
                        CustomerName = model.CustomerName,
                        PhoneNumber = model.PhoneNumber,
                        City = model.City,
                        District = model.District,
                        Address = model.Address,
                        Note = model.Note
                    };

                    var isOrderAdded = await _orderService.AddOrderAsync(order);

                    if (isOrderAdded != 0)
                    {
                        Session.Remove("Cart");

                        foreach (var orderDetail in cart)
                        {
                            orderDetail.OrderId = isOrderAdded;
                            orderDetail.Product = null;

                            await _orderDetailService.AddOrderDetailAsync(orderDetail);
                        }

                        return RedirectToAction("Cart");
                    }
                    else
                    {
                        // Handle error when adding order
                    }
                }
            }
            return View("Checkout", model);
        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}