using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        public async Task<ActionResult> Index()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            return View(banners);
        }
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Banner banner, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName);
                    imageFile.SaveAs(imagePath);
                    banner.ImageUrl = imageName;
                }

                banner.CreatedAt = DateTime.Now;
                var result = await _bannerService.AddBannerAsync(banner);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(banner);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var banner = await _bannerService.GetBannerByIdAsync(id.Value);
            if (banner == null)
            {
                return HttpNotFound();
            }

            return View(banner);
        }
        // POST: Admin/Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Banner banner, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName);
                    imageFile.SaveAs(imagePath);
                    banner.ImageUrl = imageName;
                }

                var result = await _bannerService.UpdateBannerAsync(banner);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(banner);
        }
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _bannerService.DeleteBannerAsync(id);
            return Json(new { success = result });
        }
    }
}
