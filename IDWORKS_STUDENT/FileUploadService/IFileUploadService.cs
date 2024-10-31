using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using IDWORKS_STUDENT.Models;
namespace IDWORKS_STUDENT
{
   public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
        List<ExcelDataInfo.Section> ReadExcel(IFormFile file);
        List<ExcelDataInfo.Section> ReadCSV(IFormFile file);
    }
}
