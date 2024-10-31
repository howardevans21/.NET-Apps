using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWORKS_STUDENT.Models;

namespace IDWORKS_STUDENT.Repository
{
    public interface IStudentRepository
    {
        Task<int> AddNewStudent(Colina_Student_Card_Accident model);

        bool ShouldRecordBeAdded(Colina_Student_Card_Accident model);

        Task<List<Colina_Student_Card_Accident>> GetAllStudents();
        Task<Colina_Student_Card_Accident> GetStudentById(int id);

        Task<List<Colina_Student_Card_Accident>> GetStudents(List<int> uniqueIdentifiers);
        List<Colina_Student_Card_Accident> SearchStudent(string searchStr);
    }
}
