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
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: News
        public async Task<ActionResult> Index()
        {
            var news = await _newsService.GetAllNewsAsync();
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(News news, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName);
                    imageFile.SaveAs(imagePath);
                    news.ImageUrl = imageName;
                }

                news.CreatedAt = DateTime.Now;
                var result = await _newsService.AddNewsAsync(news);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var news = await _newsService.GetNewsByIdAsync(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(News news, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingNews = await _newsService.GetNewsByIdAsync(news.NewsId);
                if (existingNews == null)
                {
                    return HttpNotFound();
                }

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existingNews.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(Server.MapPath("~/Content/Images/"), existingNews.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageName);
                    imageFile.SaveAs(imagePath);
                    news.ImageUrl = imageName;
                }
                else
                {
                    // Retain the old image URL if no new image is uploaded
                    news.ImageUrl = existingNews.ImageUrl;
                }

                var result = await _newsService.UpdateNewsAsync(news);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost]
    
        public async Task<JsonResult> Delete(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return Json(new { success = false });
            }

            var result = await _newsService.DeleteNewsAsync(id);
            if (result)
            {
                DeleteImageFile(news.ImageUrl);
            }

            return Json(new { success = result });
        }

        private void DeleteImageFile(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imagePath = Path.Combine(Server.MapPath("~/Content/Images/"), imageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}