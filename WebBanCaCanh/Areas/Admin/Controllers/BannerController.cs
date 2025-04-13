using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ cho phép người dùng có vai trò "Admin" truy cập
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService; // Dịch vụ banner

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<ActionResult> Index()
        {
            var banners = await _bannerService.GetAllBannersAsync(); // Lấy tất cả banner
            return View(banners); // Trả về view với danh sách banner
        }

        public ActionResult Create()
        {
            return View(); // Trả về view để tạo banner mới
        }

        // POST: Admin/Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Banner banner, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName); // Tạo tên ảnh duy nhất
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName); // Đường dẫn lưu ảnh
                    imageFile.SaveAs(imagePath); // Lưu ảnh vào server
                    banner.ImageUrl = imageName; // Lưu tên ảnh vào model banner
                }

                banner.CreatedAt = DateTime.Now; // Gán ngày tạo cho banner
                var result = await _bannerService.AddBannerAsync(banner); // Thêm banner vào cơ sở dữ liệu
                if (result)
                {
                    return RedirectToAction("Index"); // Chuyển hướng về trang danh sách banner
                }
            }
            return View(banner); // Trả về view với dữ liệu banner đã nhập
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); // Trả về lỗi nếu id không hợp lệ
            }

            var banner = await _bannerService.GetBannerByIdAsync(id.Value); // Lấy thông tin banner theo id
            if (banner == null)
            {
                return HttpNotFound(); // Trả về lỗi nếu không tìm thấy banner
            }

            return View(banner); // Trả về view với thông tin banner
        }

        // POST: Admin/Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Banner banner, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu
            {
                var existingBanner = await _bannerService.GetBannerByIdAsync(banner.BannerId); // Lấy thông tin banner hiện tại theo id
                if (existingBanner == null)
                {
                    return HttpNotFound(); // Trả về lỗi nếu không tìm thấy banner
                }

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingBanner.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(Server.MapPath("~/Content/Images/"), existingBanner.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu ảnh mới
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName); // Tạo tên ảnh duy nhất
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName); // Đường dẫn lưu ảnh
                    imageFile.SaveAs(imagePath); // Lưu ảnh vào server
                    banner.ImageUrl = imageName; // Lưu tên ảnh mới vào model banner
                }
                else
                {
                    // Giữ lại URL ảnh cũ nếu không tải lên ảnh mới
                    banner.ImageUrl = existingBanner.ImageUrl;
                }

                var result = await _bannerService.UpdateBannerAsync(banner); // Cập nhật thông tin banner
                if (result)
                {
                    return RedirectToAction("Index"); // Chuyển hướng về trang danh sách banner
                }
            }
            return View(banner); // Trả về view với dữ liệu banner đã cập nhật
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id); // Lấy thông tin banner theo id
            if (banner == null)
            {
                return Json(new { success = false }); // Trả về kết quả thất bại nếu không tìm thấy banner
            }

            var result = await _bannerService.DeleteBannerAsync(id); // Xóa banner
            if (result)
            {
                DeleteImageFile(banner.ImageUrl); // Xóa ảnh của banner
            }

            return Json(new { success = result }); // Trả về kết quả thành công hoặc thất bại
        }

        private void DeleteImageFile(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageUrl); // Đường dẫn ảnh
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath); // Xóa ảnh nếu tồn tại
                }
            }
        }
    }
}
