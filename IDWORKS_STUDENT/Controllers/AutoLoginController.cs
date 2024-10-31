using IDWORKS_STUDENT.AppConfig;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.Controllers
{
    public class AutoLoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
      
        public AutoLoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            string userOSName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower();
            string[] splitOSUserName = userOSName.Split("\\");
            string userName = "";
            string domain = "";
            if (splitOSUserName.Length > 0)
                domain = splitOSUserName[0];
            if (splitOSUserName.Length >= 1)
                userName = splitOSUserName[1];

            string userNameEntered = string.Format("{0}@{1}.com", userName, domain);
            string password = string.Format("{0}@{1}.com0P", userName, domain);
            var user = await _userManager.FindByNameAsync(userNameEntered);
            user.EmailConfirmed = true;
            await _signInManager.SignOutAsync();

            var principalUser = await _signInManager.CreateUserPrincipalAsync(user);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principalUser, authProperties);

            await _signInManager.SignInAsync(user, isPersistent: false);

            var isUserSignedIn = _signInManager.IsSignedIn(principalUser);
            var roleSuperAdmin = principalUser.IsInRole("SuperAdmin");
            var roleAdmin = principalUser.IsInRole("Admin");
            var roleBasic = principalUser.IsInRole("Basic");
            var roleModerator = principalUser.IsInRole("Moderator");

            UserSignAuth.userAlreadySignedIn = isUserSignedIn;
            UserSignAuth.RoleSuperAdmin = roleSuperAdmin;
            UserSignAuth.RoleAdmin = roleAdmin;
            UserSignAuth.RoleBasic = roleBasic;
            UserSignAuth.RoleModerator = roleModerator;

            string mainURL = "~/Index";
            string homePage = "~/Index";
            string studentPage = "~/StudentListPage";
            string rolesPage = "~/RoleManager/ListRoles";
            string userRolesPage = "~/searchUser";

            switch (UserSignAuth.WhichSignedOnPage)
            {
                case UserSignAuth.SignedOnPage.StudentPageList:
                    mainURL = studentPage;
                    break;
                case UserSignAuth.SignedOnPage.StudentUploadFile:
                    mainURL = homePage;
                    break;
                case UserSignAuth.SignedOnPage.Roles:
                    mainURL = rolesPage;
                    break;
                case UserSignAuth.SignedOnPage.UsersRoles:
                    mainURL = userRolesPage;
                    break;
            }
     
            return Redirect(mainURL);
        }
    }
}
