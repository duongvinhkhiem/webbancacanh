using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;
        private readonly IProductService _productService;
        private readonly IProductPromotionService _productPromotionService;
        public PromotionController(IPromotionService promotionService, IProductService productService, IProductPromotionService productPromotionService)
        {
            _promotionService = promotionService;
            _productService = productService;
            _productPromotionService = productPromotionService;
        }

        // GET: Promotion
        public async Task<ActionResult> Index()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return View(promotions);
        }

        // GET: Promotion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promotion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.CreatedDate = DateTime.Now;
                var result = await _promotionService.AddPromotionAsync(promotion);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(promotion);
        }

        // GET: Promotion/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var promotion = await _promotionService.GetPromotionByIdAsync(id.Value);
            if (promotion == null)
            {
                return HttpNotFound();
            }

            return View(promotion);
        }

        // POST: Promotion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var result = await _promotionService.UpdatePromotionAsync(promotion);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(promotion);
        }

        // POST: Promotion/Delete/5
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            return Json(new { success = result });
        }
       


    }
}