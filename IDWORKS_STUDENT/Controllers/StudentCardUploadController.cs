using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using IDWORKS_STUDENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IDWORKS_STUDENT.AppConfig;

namespace IDWORKS_STUDENT.Controllers
{
    public class StudentCardUploadController : Controller
    {
        public IActionResult Index()
        {
            UserSignAuth.WhichSignedOnPage = UserSignAuth.SignedOnPage.StudentUploadFile;
            return View();
        } 

        public IActionResult ShowPage()
        {

            return View();
        }
    }
}
