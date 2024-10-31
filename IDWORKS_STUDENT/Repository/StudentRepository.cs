using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWORKS_STUDENT.Models;
using Microsoft.EntityFrameworkCore;

namespace IDWORKS_STUDENT.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context = null;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewStudent(Colina_Student_Card_Accident studentModel)
        {
            string formattedDate = studentModel.IDWEffectiveDate.ToString("MM/dd/yyyy");
            var newStudent = new Colina_Student_Card_Accident()
            {
                IDWFirstName = studentModel.IDWFirstName,
                IDWLastName = studentModel.IDWLastName,
                IDWCoverageProvider = studentModel.IDWCoverageProvider,
                IDWPolicyNumber = studentModel.IDWPolicyNumber,
                IDWEffectiveDate = studentModel.IDWEffectiveDate,
                IDWEffectiveDateText = formattedDate,
                IDWSchool = studentModel.IDWSchool,
                IDWCoverage = studentModel.IDWCoverage
            };

            await _context.Colina_Student_Card_Accident.AddAsync(newStudent);
            await _context.SaveChangesAsync();

            return newStudent.Id;
        }

        public bool ShouldRecordBeAdded(Colina_Student_Card_Accident studentModel)
        {
            bool isValid = true;

            string policyNumber = studentModel.IDWPolicyNumber.ToLower().Replace(" ", "").Trim();
            string firstName = studentModel.IDWFirstName.ToLower().Replace(" ", "").Trim();
            string lastName = studentModel.IDWLastName.ToLower().Replace(" ", "").Trim();
            string coverageProvider = studentModel.IDWCoverageProvider.ToLower().Replace(" ", "").Trim();
            string school = studentModel.IDWSchool.ToLower().Replace(" ", "").Trim();
            string coverage = studentModel.IDWCoverage.ToLower().Replace(" ", "").Trim();

            int day = studentModel.IDWEffectiveDate.Day;
            int month = studentModel.IDWEffectiveDate.Month;
            int year = studentModel.IDWEffectiveDate.Year;


            int studentRecordCount = _context.Colina_Student_Card_Accident.Where(x => x.IDWPolicyNumber.ToLower().Replace(" ", "").Trim() == policyNumber
                            && x.IDWFirstName.ToLower().Replace(" ", "").Trim() == firstName
                            && x.IDWLastName.ToLower().Replace(" ", "").Trim() == lastName
                            && x.IDWCoverageProvider.ToLower().Replace(" ", "").Trim() == coverageProvider
                            && x.IDWEffectiveDate.Date.Day == day
                            && x.IDWEffectiveDate.Date.Month == month
                            && x.IDWEffectiveDate.Date.Year == year
                            && x.IDWCoverage.ToLower().Replace(" ", "").Trim() == coverage
                            && x.IDWSchool.ToLower().Replace(" ", "").Trim() == school).Count();

            if (studentRecordCount > 0)
                isValid = false;

            return isValid;
        }

        public async Task<List<Colina_Student_Card_Accident>> GetAllStudents()
        {
            return await _context.Colina_Student_Card_Accident
                    .Select(student => new Colina_Student_Card_Accident()
                    {
                        IDWFirstName = student.IDWFirstName,
                        IDWLastName = student.IDWLastName,
                        IDWPolicyNumber = student.IDWPolicyNumber,
                        IDWCoverageProvider = student.IDWCoverageProvider,
                        IDWEffectiveDate = student.IDWEffectiveDate,
                        IDWSchool = student.IDWSchool

                    }).ToListAsync();
        }


        /// <summary>
        /// Filter on a list of students passed in using Id
        /// </summary>
        /// <param name="studentModels"></param>
        /// <returns></returns>
        public async  Task<List<Colina_Student_Card_Accident>> GetStudents(List<int> uniqueIdentifiers)
        {
            return await _context.Colina_Student_Card_Accident.Where(x => uniqueIdentifiers.Contains(x.Id)).ToListAsync(); 
    
            // return await _context.Student_Accident_Test.Where(p => studentModels.All(p2 => p2.Id == p.Id)).ToListAsync();
        }

        public async Task<Colina_Student_Card_Accident> GetStudentById(int id)
        {
            return await _context.Colina_Student_Card_Accident.Where(x => x.Id == id)
                .Select(student => new Colina_Student_Card_Accident()
                {
                    IDWFirstName = student.IDWFirstName,
                    IDWLastName = student.IDWLastName,
                    IDWPolicyNumber = student.IDWPolicyNumber,
                    IDWCoverageProvider = student.IDWCoverageProvider,
                    IDWEffectiveDate = student.IDWEffectiveDate,
                    IDWSchool = student.IDWSchool
                }).FirstOrDefaultAsync();
        }

        List<Colina_Student_Card_Accident> IStudentRepository.SearchStudent(string searchStr)
        {
          return  _context.Colina_Student_Card_Accident.Where(x => x.IDWFirstName.Contains(searchStr) || x.IDWLastName.Contains(searchStr) ||
                        x.IDWPolicyNumber.Contains(searchStr) || x.IDWSchool.Contains(searchStr)).ToList();
        }
    }
}
