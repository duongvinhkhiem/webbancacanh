using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBanCaCanh.Models;

namespace WebBanCaCanh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersAdminController()
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            _userManager = new ApplicationUserManager(userStore);

            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View(users);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ApplicationUser user, string role, string password)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        result = await _userManager.AddToRoleAsync(user.Id, role);
                        if (!result.Succeeded)
                        {
                            return Json(new { success = false, errors = GetErrors(result) });
                        }
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = GetModelErrors() });
        }

        [HttpPost]
        public async Task<JsonResult> Edit(ApplicationUser user, string role, string password)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return Json(new { success = false, errors = new[] { "User not found." } });
                }

                existingUser.Email = user.Email;
                existingUser.UserName = user.UserName;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        var removePasswordResult = await _userManager.RemovePasswordAsync(existingUser.Id);
                        if (!removePasswordResult.Succeeded)
                        {
                            return Json(new { success = false, errors = GetErrors(removePasswordResult) });
                        }
                        var addPasswordResult = await _userManager.AddPasswordAsync(existingUser.Id, password);
                        if (!addPasswordResult.Succeeded)
                        {
                            return Json(new { success = false, errors = GetErrors(addPasswordResult) });
                        }
                    }

                    var roles = await _userManager.GetRolesAsync(existingUser.Id);
                    await _userManager.RemoveFromRolesAsync(existingUser.Id, roles.ToArray());

                    if (!string.IsNullOrEmpty(role))
                    {
                        result = await _userManager.AddToRoleAsync(existingUser.Id, role);
                        if (!result.Succeeded)
                        {
                            return Json(new { success = false, errors = GetErrors(result) });
                        }
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = GetModelErrors() });
        }


        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errors = GetErrors(result) });
            }
            return Json(new { success = false, errors = new[] { "User not found." } });
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
