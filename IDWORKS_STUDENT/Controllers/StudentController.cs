using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IDWORKS_STUDENT.Models;
using IDWORKS_STUDENT.Repository;

namespace IDWORKS_STUDENT.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository = null;
        private IWebHostEnvironment _webHostEnvironment;
    
        public StudentController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
           
        }

    }

}
