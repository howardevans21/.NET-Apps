using System;
using System.ComponentModel.DataAnnotations;

namespace IDWORKS_STUDENT.Models
{
    public class Colina_Student_Card_Accident
    {
        public int Id { get; set; }

        [Required]
        public string IDWFirstName { get; set; }
        [Required]
        public string IDWLastName { get; set; }

        [Required]
        public string IDWSchool { get; set; }
        [Required]
        public string IDWPolicyNumber { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime IDWEffectiveDate { get; set; }

        public string IDWEffectiveDateText { get; set; }

        [Required]
        public string IDWCoverageProvider { get; set; }

        [Required]
        public string IDWCoverage { get; set; }
    }
}
