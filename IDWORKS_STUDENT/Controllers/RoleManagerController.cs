using IDWORKS_STUDENT.AppConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.Controllers
{
    public class RoleManagerController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RoleManagerController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public List<IdentityRole> Roles = new List<IdentityRole>();
        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            UserSignAuth.WhichSignedOnPage = UserSignAuth.SignedOnPage.Roles;
            Roles = await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshRoles()
        {
            Roles = await _roleManager.Roles.ToListAsync();
            return View("ListRoles", Roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            bool isValid = validateRole(roleName, true);
            if (roleName != null && isValid)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            } else
            {
                ModelState.AddModelError("invalidRole", "The role entered is invalid. It needs to either be Admin, SuperAdmin, Basic, or Moderator");
            }
            return RedirectToAction("ListRoles");
        }

        private bool validateRole(string roleName, bool userUpper)
        {
            string r = roleName.Trim();
            bool valid = false;

            if (!userUpper)
            {
                if (r == "Basic" || r == "SuperAdmin" || r == "Moderator" || r == "Admin")
                    valid = true;
            } else
            {
                if (r.ToUpper() == "BASIC" || r.ToUpper() == "SUPERADMIN" || r.ToUpper() == "MODERATOR" || r.ToUpper() == "ADMIN")
                    valid = true;
            }

            return valid;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {

                return RedirectToAction("ListRoles");
            }

            var users = await _userManager.GetUsersInRoleAsync(role.Name);

            bool isValid = validateRole(role.Name, true);
            if (users.Count > 0 && isValid)
            {
                ModelState.AddModelError("deleteRoleError", "Cannot remove existing role " + role.Name + " because there are users are associated to the role or the role is required.");

                return View("ListRoles", Roles);
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("cannotRemoveRole", "There was a problem removing existing role.");

                return View("ListRoles", Roles);
            }

            return RedirectToAction("ListRoles");
        }
    }
}
