using IDWORKS_STUDENT.AppConfig;
using IDWORKS_STUDENT.Models;
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
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            UserSignAuth.WhichSignedOnPage = UserSignAuth.SignedOnPage.UsersRoles;

            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<Models.UserRolesViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.UserName = user.UserName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }

        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [HttpGet]
        public IActionResult ManageAddUser()
        {
            return View("ManageAddUser");
        }

        public async Task<IActionResult> ManageAddUser(string userNameEntered)
        {
            string password = userNameEntered + "0P";
            var user = new IdentityUser { UserName = userNameEntered, Email = password };

            var result = await _userManager.CreateAsync(user, user.Email);
            bool isValid = user.Email.Contains("@colina.com");
            if (result.Succeeded && isValid)
            {
                await _userManager.AddToRoleAsync(user, IDWORKS_STUDENT.Pages.Role.Enums.Roles.Basic.ToString());
            } else
            {
                ModelState.AddModelError("invalidUser", "The user you attempted to add does not have a valid email address or the email address does not belong to Colina. Only Colina email addresses are allowed.");
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
              
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                } 
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var result =  await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing user\r\n Please ensure all roles are removed before deleting the user");
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

        #region Search


        [HttpGet("searchUser")]
        public async Task<IActionResult> Index(List<ManageUserRolesViewModel> model, string searchUser, string searchEmail)
        {
            UserSignAuth.WhichSignedOnPage = UserSignAuth.SignedOnPage.UsersRoles;
            int pageSize = 4;
            ViewBag.CurrentFilter = searchUser;
            searchEmail = searchEmail != null ? searchEmail : "";
            searchUser = searchUser != null ? searchUser : "";
          
            if (searchUser.Length > 0 && searchEmail.Length == 0) { 
                var modelFilter = new List<Models.UserRolesViewModel>();
                var users = _userManager.Users;
                var userList = await users.Where(x => x.UserName.ToLower().Contains(searchUser.ToLower()) || x.UserName.ToLower() == searchUser.ToLower()).ToListAsync();

                foreach (var user in userList)
                {
                        var userRolesViewModel = new Models.UserRolesViewModel
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            UserId = user.Id,
                            Roles = await GetUserRoles(user)
                        };

                        modelFilter.Add(userRolesViewModel);
                }
                return View(modelFilter);
            } else if (searchEmail.Length > 0 && searchEmail.Length == 0)
            {
                var modelFilter = new List<Models.UserRolesViewModel>();
                var users = _userManager.Users;
                var userList = await users.Where(x => x.Email.ToLower().Contains(searchEmail.ToLower()) || x.Email.ToLower() == searchEmail.ToLower()).ToListAsync();

                foreach (var user in userList)
                {
                    var userRolesViewModel = new Models.UserRolesViewModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id,
                        Roles = await GetUserRoles(user)
                    };

                    modelFilter.Add(userRolesViewModel);
                }
                return View(modelFilter);
            } 
            
            else if(searchUser.Length > 0 && searchEmail.Length > 0)
            {
                var modelFilter = new List<Models.UserRolesViewModel>();
                var users = _userManager.Users;
                var userList = await users.Where(x => x.UserName.ToLower().Contains(searchUser.ToLower()) || x.UserName.ToLower() == searchUser.ToLower()
                 && (x.Email.ToLower() == searchEmail.ToLower() || x.Email.Contains(searchEmail))
                ).ToListAsync();

                foreach (var user in userList)
                {
                    var userRolesViewModel = new Models.UserRolesViewModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id,
                        Roles = await GetUserRoles(user)
                    };

                    modelFilter.Add(userRolesViewModel);
                }
                return View(modelFilter);
            } else
            {
                var users = await _userManager.Users.ToListAsync();
                var userRolesViewModel = new List<Models.UserRolesViewModel>();
                foreach (IdentityUser user in users)
                {
                    var thisViewModel = new UserRolesViewModel();
                    thisViewModel.UserId = user.Id;
                    thisViewModel.Email = user.Email;
                    thisViewModel.UserName = user.UserName;
                    thisViewModel.Roles = await GetUserRoles(user);
                    userRolesViewModel.Add(thisViewModel);
                }

                return View(userRolesViewModel);
            }
        }

        #endregion 
    }
}
