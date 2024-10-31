using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using IDWORKS_STUDENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace IDWORKS_STUDENT
{

    public class PostFile : PageModel
    {
        private readonly ILogger<PostFile> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IDWORKS_STUDENT.Repository.IStudentRepository _studentRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public string FilePath = "";
        private List<Colina_Student_Card_Accident> _addedStudentModels = new List<Colina_Student_Card_Accident>();
        private List<ExcelDataInfo.ErrorInfoCell> _cellErrors = new List<ExcelDataInfo.ErrorInfoCell>();

        public List<Colina_Student_Card_Accident> AddedStudentModels
        {
            get { return _addedStudentModels; }
        }

        public List<ExcelDataInfo.ErrorInfoCell> CellErrors
        {
            get { return _cellErrors; }
        }

        public PostFile(ILogger<PostFile> logger, IFileUploadService fileUploadService, IDWORKS_STUDENT.Repository.IStudentRepository studentRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this._fileUploadService = fileUploadService;
            this._studentRepository = studentRepository;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public  void OnGet()
        {
           // await HttpContext.SignOutAsync(
            // CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> OnPostSubmit(IFormFile file)
        {
            List<int> uniqueIdentifiers = new List<int>();
            if (file != null && checkValidFile(file))
            {
                FilePath = await _fileUploadService.UploadFileAsync(file);

                if (isExcel(file))
                {
                    List<ExcelDataInfo.Section> sections = _fileUploadService.ReadExcel(file);

                    foreach (ExcelDataInfo.Section section in sections)
                    {

                        if (section.ColumnsInCorrectlyFormatted)
                            ModelState.AddModelError("incorrectExcelColumnFormat", "There are one or more columns in the Excel worksheet that is incorrectly formmatted. " +
                                "Please ensure all columns do not have any gaps and they are properly aligned by following the agreed specified format.");

                        List<Colina_Student_Card_Accident> studentModels = createStudentModels(section.dataInfoCells);
                        _cellErrors = createErrorModels(sections);

                        foreach (Colina_Student_Card_Accident studentModel in studentModels)
                        {
                            bool isValid = _studentRepository.ShouldRecordBeAdded(studentModel);
                            if (isValid)
                            {
                                int uniqueIdentifier = await _studentRepository.AddNewStudent(studentModel);
                                uniqueIdentifiers.Add(uniqueIdentifier);
                            }
                            else
                            {
                                ModelState.AddModelError("alreadyAdded", "There were records already added from the file you've uploaded.");
                            }
                        }
                    }
                    _addedStudentModels = await _studentRepository.GetStudents(uniqueIdentifiers);

                }
                else if (isCSV(file))
                {
                    List<ExcelDataInfo.Section> csv_sections = _fileUploadService.ReadCSV(file);
                    List<Colina_Student_Card_Accident> csv_StudentModels = createStudentModels(csv_sections[0].dataInfoCells);
                    _cellErrors = createErrorModels(csv_sections);

                    foreach (Colina_Student_Card_Accident studentModel in csv_StudentModels)
                    {
                        bool isValid = _studentRepository.ShouldRecordBeAdded(studentModel);
                        if (isValid)
                        {
                            int uniqueIdentifier = await _studentRepository.AddNewStudent(studentModel);
                            uniqueIdentifiers.Add(uniqueIdentifier);
                        }
                        else
                        {
                            ModelState.AddModelError("alreadyAdded", "There were records already added from the file you've uploaded.");
                        }
                    }
                    _addedStudentModels = await _studentRepository.GetStudents(uniqueIdentifiers);
                }

            }
            else
                ModelState.AddModelError("invalidFileType", "The file type is invalid or a file was not selected. Please upload only Excel or CSV files");

            return Page();
        }

        private List<ExcelDataInfo.ErrorInfoCell> createErrorModels(List<ExcelDataInfo.Section> sections)
        {
            List<ExcelDataInfo.ErrorInfoCell> errorList = new List<ExcelDataInfo.ErrorInfoCell>();
            foreach (ExcelDataInfo.Section section in sections)
            {
                foreach (ExcelDataInfo.ErrorInfoCell errorInfoCell in section.errorInfoCells)
                {
                    errorList.Add(errorInfoCell);
                }
            }
            return errorList;
        }

        private List<Colina_Student_Card_Accident> createStudentModels(List<ExcelDataInfo.DataInfoCell> dataInfoCells)
        {
            List<Colina_Student_Card_Accident> studentModels = new List<Colina_Student_Card_Accident>();
            foreach (ExcelDataInfo.DataInfoCell dataInfoCell in dataInfoCells)
            {
                Colina_Student_Card_Accident studentModel = new Colina_Student_Card_Accident();
                studentModel.IDWCoverageProvider = dataInfoCell.CoverageProvider;
                studentModel.IDWEffectiveDate = dataInfoCell.EffectiveDate;
                studentModel.IDWFirstName = dataInfoCell.FirstName;
                studentModel.IDWLastName = dataInfoCell.LastName;
                studentModel.IDWPolicyNumber = dataInfoCell.PolicyNumber;
                studentModel.IDWSchool = dataInfoCell.School;
                studentModel.IDWCoverage = dataInfoCell.Coverage;
                studentModels.Add(studentModel);
            }

            return studentModels;
        }

        private bool checkValidFile(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            bool isValid = false;
            if (extension == ".csv" || extension == ".xlsx" || extension == ".xls") { isValid = true; }
            return isValid;
        }

        private bool isExcel(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            bool isValid = false;
            if (extension == ".xlsx" || extension == ".xls") { isValid = true; }
            return isValid;
        }

        private bool isCSV(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            bool isValid = false;
            if (extension == ".csv") { isValid = true; }
            return isValid;
        }
    }
}
