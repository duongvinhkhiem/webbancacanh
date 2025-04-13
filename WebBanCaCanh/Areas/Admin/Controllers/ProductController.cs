using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ cho phép người dùng có vai trò "Admin" truy cập
    public class ProductController : Controller
    {
        private readonly IProductService _productService; // Dịch vụ sản phẩm
        private readonly ICategoryService _categoryService; // Dịch vụ danh mục

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<ActionResult> Index(int? page)
        {
            const int pageSize = 5; // Số lượng sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, mặc định là 1

            var products = await _productService.GetAllProductsAsync(); // Lấy tất cả sản phẩm
            var pagedProducts = products.ToPagedList(pageNumber, pageSize); // Phân trang sản phẩm

            return View(pagedProducts); // Trả về view với danh sách sản phẩm đã phân trang
        }

        // GET: Product/Create
        public async Task<ActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync(); // Lấy tất cả danh mục
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName"); // Gửi danh mục đến view
            return View(); // Trả về view để tạo sản phẩm mới
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, IEnumerable<HttpPostedFileBase> imageFiles)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu
            {
                // Thêm sản phẩm vào cơ sở dữ liệu
                var result = await _productService.AddProductAsync(product);
                if (result)
                {
                    // Xử lý các tệp ảnh
                    if (imageFiles != null)
                    {
                        foreach (var file in imageFiles)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Tạo tên ảnh duy nhất
                                var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName); // Đường dẫn lưu ảnh
                                file.SaveAs(imagePath); // Lưu ảnh vào server

                                var productImage = new ProductImage
                                {
                                    ProductId = product.ProductId,
                                    ImageUrl = imageName
                                };
                                await _productService.AddProductImageAsync(productImage); // Thêm thông tin ảnh vào cơ sở dữ liệu
                            }
                        }
                    }
                    return RedirectToAction("Index"); // Chuyển hướng về trang danh sách sản phẩm
                }
            }
            var categories = await _categoryService.GetAllCategoriesAsync(); // Lấy lại danh mục nếu có lỗi
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product); // Trả về view với dữ liệu sản phẩm đã nhập
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); // Trả về lỗi nếu id không hợp lệ
            }

            var product = await _productService.GetProductByIdAsync(id.Value); // Lấy thông tin sản phẩm theo id
            if (product == null)
            {
                return HttpNotFound(); // Trả về lỗi nếu không tìm thấy sản phẩm
            }
            var categories = await _categoryService.GetAllCategoriesAsync(); // Lấy tất cả danh mục
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName"); // Gửi danh mục đến view
            return View(product); // Trả về view với thông tin sản phẩm
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, IEnumerable<HttpPostedFileBase> imageFiles)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu
            {
                // Cập nhật thông tin sản phẩm
                var result = await _productService.UpdateProductAsync(product);
                if (result)
                {
                    // Xử lý các tệp ảnh
                    if (imageFiles != null)
                    {
                        foreach (var file in imageFiles)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Tạo tên ảnh duy nhất
                                var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName); // Đường dẫn lưu ảnh
                                file.SaveAs(imagePath); // Lưu ảnh vào server

                                var productImage = new ProductImage
                                {
                                    ProductId = product.ProductId,
                                    ImageUrl = imageName
                                };
                                await _productService.AddProductImageAsync(productImage); // Thêm thông tin ảnh vào cơ sở dữ liệu
                            }
                        }
                    }
                    return RedirectToAction("Index"); // Chuyển hướng về trang danh sách sản phẩm
                }
            }
            var categories = await _categoryService.GetAllCategoriesAsync(); // Lấy lại danh mục nếu có lỗi
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product); // Trả về view với dữ liệu sản phẩm đã cập nhật
        }

        // POST: Product/Delete/5
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id); // Xóa sản phẩm theo id
            return Json(new { success = result }); // Trả về kết quả dưới dạng JSON
        }

        // POST: Product/DeleteImage/5
        [HttpPost]
        public async Task<JsonResult> DeleteImage(int imageId)
        {
            var result = await _productService.DeleteProductImageAsync(imageId); // Xóa ảnh sản phẩm theo id ảnh
            return Json(new { success = result }); // Trả về kết quả dưới dạng JSON
        }
    }
}
