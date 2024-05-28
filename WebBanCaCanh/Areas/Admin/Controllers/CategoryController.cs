using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        // Constructor sẽ nhận một thể hiện của ICategoryService thông qua DI
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public async Task<ActionResult> Index()
        {
            // Gọi phương thức GetAllCategoriesAsync từ _categoryService để lấy tất cả các danh mục
            var categories = await _categoryService.GetAllCategoriesAsync();

            // Trả về view với danh sách các danh mục
            return View(categories);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Thêm danh mục mới vào cơ sở dữ liệu
                var result = await _categoryService.AddCategoryAsync(category);
                if (result)
                {
                    // Nếu thêm thành công, chuyển hướng về trang Index
                    return RedirectToAction("Index");
                }
            }
            // Nếu có lỗi xảy ra, hiển thị lại form Create với thông báo lỗi
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tìm danh mục cần chỉnh sửa
            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            // Trả về view với danh mục cần chỉnh sửa
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
      
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin danh mục
                var result = await _categoryService.UpdateCategoryAsync(category);
                if (result)
                {
                    // Nếu cập nhật thành công, chuyển hướng về trang Index
                    return RedirectToAction("Index");
                }
            }
            // Nếu có lỗi xảy ra, hiển thị lại form Edit với thông báo lỗi
            return View(category);
        }

        [HttpPost]

        public async Task<JsonResult> Delete(int id)
        {
            // Xác nhận xóa danh mục
            var result = await _categoryService.DeleteCategoryAsync(id);

            // Return a JSON response indicating success or failure
            return Json(new { success = result });
        }
    }
}