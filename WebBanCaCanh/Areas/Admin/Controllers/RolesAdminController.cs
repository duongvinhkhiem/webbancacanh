using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesAdminController()
        {
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        public ActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<JsonResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = GetModelErrors() });
        }

        [HttpPost]
        public async Task<JsonResult> Edit(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = GetModelErrors() });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = new[] { "Role not found." } });
        }

        private string[] GetErrors(IdentityResult result)
        {
            return result.Errors.ToArray();
        }

        private string[] GetModelErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
        }
    }
}
